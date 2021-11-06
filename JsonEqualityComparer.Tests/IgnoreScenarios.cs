using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonEqualityComparer.Tests
{
    [TestFixture]
    public class IgnoreScenarios
    {
        [Test]
        public void WhenJsonObjectsAreIdentical_ShouldMatch()
        {
            var actual = @"{
  ""property1"": {
    ""subProperty"": { ""someOption"": false }
    },
  ""property2"": {
    ""subProperty"": { ""someOption"": false }
    }
}";

            var expected = @"{
  ""property1"": {
    ""subProperty"": { ""someOption"": false }
    },
  ""property2"": {
    ""subProperty"": { ""someOption"": false }
    }
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
        public void WhenJsonObjectsIncludeAnIgnoredProperty_ShouldMatch()
        {
            var actual = @"{
  ""property1"": {
    ""subProperty"": { ""someOption"": false }
    },
  ""property2"": {
    ""subProperty"": { ""someOption"": false }
    }
}";

            var expected = @"{
  ""property1"": {
    ""$ComparisonOptions"": { ""Ignore"": true }
    },
  ""property2"": {
    ""subProperty"": { ""someOption"": false }
    }
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
        public void WhenJsonObjectsDontIncludeAnIgnoredProperty_ShouldMatch()
        {
            var actual = @"{
  ""property2"": {
    ""subProperty"": { ""someOption"": false }
    }
}";

            var expected = @"{
  ""property1"": {
    ""$ComparisonOptions"": { ""Ignore"": true }
    },
  ""property2"": {
    ""subProperty"": { ""someOption"": false }
    }
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

        private JToken Deserialise(string json)
        {
            return (JToken)JsonConvert.DeserializeObject(json);
        }
    }
}
