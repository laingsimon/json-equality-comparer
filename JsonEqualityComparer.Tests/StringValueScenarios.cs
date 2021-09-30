using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace JsonEqualityComparer.Tests
{
    [TestFixture]
    public class StringValueScenarios
    {
        [TestCase(true)]
        [TestCase(false)]
        public void WhenJsonObjectsHaveIdenticalLineEndings_ShouldMatch(bool ignoreLineEndingDifferences)
        {
            var actual = "{ \"string property\": \"line 1\r\nline 2\" }";
            var expected = "{ \"string property\": \"line 1\r\nline 2\" }";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    StringValueComparer = ignoreLineEndingDifferences
                        ? new IgnoreLineEndingsComparer()
                        : (IEqualityComparer<string>)StringComparer.Ordinal,
                }
            };

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(context.Differences, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WhenJsonObjectsHaveIdenticalCasing_ShouldMatch(bool ignoreLineCasingDifferences)
        {
            var actual = "{ \"string property\": \"line 1\r\nline 2\" }";
            var expected = "{ \"string property\": \"line 1\r\nline 2\" }";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    StringValueComparer = ignoreLineCasingDifferences
                        ? StringComparer.OrdinalIgnoreCase
                        : StringComparer.Ordinal,
                }
            };

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(context.Differences, Is.Empty);
        }

        [Test]
        public void WhenJsonObjectsHaveDifferentLineEndings_ShouldNotMatch()
        {
            var actual = "{ \"string property\": \"line 1\r\nline 2\" }";
            var expected = "{ \"string property\": \"line 1\nline 2\" }";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext();

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(context.Differences, Is.Not.Empty);
        }

        [Test]
        public void WhenJsonObjectsHaveDifferentCasing_ShouldNotMatch()
        {
            var actual = "{ \"string property\": \"line 1\r\nline 2\" }";
            var expected = "{ \"string property\": \"line 1\r\nLINE 2\" }";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext();

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(context.Differences, Is.Not.Empty);
        }

        [Test]
        public void WhenJsonObjectsHaveDifferentLineEndingsAndDifferencesAreIgnored_ShouldMatch()
        {
            var actual = "{ \"string property\": \"line 1\r\nline 2\" }";
            var expected = "{ \"string property\": \"line 1\nline 2\" }";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    StringValueComparer = new IgnoreLineEndingsComparer(),
                }
            };

            comparer.Compare(Deserialise(actual), Deserialise(expected), context);

            Assert.That(context.Differences, Is.Empty);
        }

        [Test]
        public void WhenJsonObjectsHaveDifferentCasingAndDifferencesAreIgnored_ShouldMatch()
        {
            var actual = "{ \"string property\": \"line 1\r\nline 2\" }";
            var expected = "{ \"string property\": \"line 1\r\nLINE 2\" }";

            var comparer = new JsonEqualityComparer();
            var context = new ComparisonContext
            {
                DefaultComparisonOptions =
                {
                    StringValueComparer = StringComparer.OrdinalIgnoreCase,
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
