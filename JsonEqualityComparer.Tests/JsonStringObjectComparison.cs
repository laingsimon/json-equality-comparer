using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Linq;

namespace JsonEqualityComparer.Tests
{
    [TestFixture]
    public class JsonStringObjectComparison
    {
        [Test]
        public void WhenTreatJsonStringsAsObjectsIsTrue_ShouldMatch()
        {
            var actual = @"{
  ""property1"": ""{ \""amount\"": 12345 }""
}";

            var expected = @"{
  ""property1"": { ""amount"": 12345 }
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
        public void WhenTreatJsonStringsAsObjectsIsTrue_ShouldReportCorrectDifferences()
        {
            var actual = @"{
  ""property1"": ""{ \""amount\"": 12345 }""
}";

            var expected = @"{
  ""property1"": { ""amount"": 12345 }
}";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    TreatJsonStringsAsObjects = false
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
