using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Linq;

namespace JsonEqualityComparer.Tests
{
    [TestFixture]
    public class CaseInsenstiveComparison
    {
        [Test]
        public void WhenCaseInsensitiveComparisonAndPropertyNamesAreADifferentCase_ShouldMatch()
        {
            var actual = @"{
  ""property1"": true
}";

            var expected = @"{
  ""Property1"": true
}";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    Explicit = true,
                    PropertyNameComparer = StringComparison.OrdinalIgnoreCase
                }
            };

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(context.Differences, Is.Empty);
        }

        [Test]
        public void WhenCaseSensitiveComparisonAndPropertyNamesAreADifferentCase_ShouldHaveCorrectDifferences()
        {
            var actual = @"{
  ""property1"": true
}";

            var expected = @"{
  ""Property1"": true
}";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext();

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(
                context.Differences.Select(d => d.ToString()),
                Is.EquivalentTo(new[] {
                    "Property1: Property was not found, in the list of property1"
                }));
        }

        private JToken Deserialise(string json)
        {
            return (JToken)JsonConvert.DeserializeObject(json);
        }
    }
}
