using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Linq;

namespace JsonEqualityComparer.Tests
{
    [TestFixture]
    public class JsonComparisonOptionsDirectives
    {
        [Test]
        public void WhenJsonPropertiesExistInActualButNotExpectedAndNotExplicit_ShouldMatch()
        {
            var actual = @"{
  ""property1"": true,
  ""property2"": ""string"",
  ""property3"": 123.45,
  ""property4"": null
}";

            var expected = @"{
  ""property1"": true,
  ""$ComparisonOptions"": { ""Explicit"": false }
}";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    Explicit = true
                }
            };

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(context.Differences, Is.Empty);
        }

        [Test]
        public void WhenJsonPropertiesExistInActualButNotExpectedAndExplicit_ShouldReportDifferences()
        {
            var actual = @"{
  ""property1"": true,
  ""property2"": ""string"",
  ""property3"": 123.45,
  ""property4"": null
}";

            var expected = @"{
  ""property1"": true,
  ""$ComparisonOptions"": { ""Explicit"": true }
}";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    Explicit = false
                }
            };

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(
                context.Differences.Select(d => d.ToString()),
                Is.EquivalentTo(new[] {
                    ": Found 3 unexpected properties: property2, property3, property4"
                }));
        }

        [Test]
        public void WhenJsonPropertyInActualContainsAJsonStringAndExpectedIsAJsonObjectAndAllowed_ShouldMatch()
        {
            var actual = @"{
  ""property1"": ""{ \""amount\"": 12345 }""
}";

            var expected = @"{
  ""property1"": { ""amount"": 12345 },
  ""$ComparisonOptions"": { ""TreatJsonStringsAsObjects"": true }
}";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    TreatJsonStringsAsObjects = true
                }
            };

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(context.Differences, Is.Empty);
        }

        [Test]
        public void WhenJsonPropertyInActualContainsAJsonStringAndExpectedIsAJsonObjectAndNotAllowed_ShouldReportDifferences()
        {
            var actual = @"{
  ""property1"": ""{ \""amount\"": 12345 }""
}";

            var expected = @"{
  ""property1"": { ""amount"": 12345 },
  ""$ComparisonOptions"": { ""TreatJsonStringsAsObjects"": false }
}";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    TreatJsonStringsAsObjects = true
                }
            };

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(
                context.Differences.Select(d => d.ToString()),
                Is.EquivalentTo(new[] {
                    "property1: Types do not match: expected Object but found String"
                }));
        }

        private JToken Deserialise(string json)
        {
            return (JToken)JsonConvert.DeserializeObject(json);
        }
    }
}
