using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Linq;

namespace JsonEqualityComparer.Tests
{
    [TestFixture]
    public class ExplicitScenarios
    {
        [Test]
        public void WhenJsonObjectsAreIdentical_ShouldMatch()
        {
            var actual = @"{
  ""property1"": true,
  ""property2"": ""string"",
  ""property3"": 123.45,
  ""property4"": null
}";

            var expected = @"{
  ""property1"": true,
  ""property2"": ""string"",
  ""property3"": 123.45,
  ""property4"": null
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
        public void WhenJsonObjectsAreDifferent_ShouldHaveCorrectDifferences()
        {
            var actual = @"{
  ""property1"": true,
  ""property2"": ""string"",
  ""property3"": 123.45,
  ""property4"": null
}";

            var expected = @"{
  ""property1"": false,
  ""property2"": ""something else"",
  ""property3"": 100,
  ""property5"": 0
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

            Assert.That(
                context.Differences.Select(d => d.ToString()),
                Is.EquivalentTo(new[] {
                    "property1: True (Boolean) does not equal False (Boolean)",
                    "property2: string (String) does not equal something else (String)",
                    "property3: Types do not match: expected Integer but found Float",
                    "property5: Property was not found, in the list of property1, property2, property3, property4",
                    ": Found 1 unexpected properties: property4"
                }));
        }

        private JToken Deserialise(string json)
        {
            return (JToken)JsonConvert.DeserializeObject(json);
        }
    }
}
