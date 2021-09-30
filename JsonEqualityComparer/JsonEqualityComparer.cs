using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer
{
    /// <summary>
    /// Compare two JSON objects and report the differences
    /// </summary>
    public class JsonEqualityComparer : IJsonEqualityComparer
    {
        public IComparisonContext Compare(JToken actual, JToken expected)
        {
            var context = new ComparisonContext();
            Compare(actual, expected, context);
            return context;
        }

        public void Compare(JToken actual, JToken expected, IComparisonContext context)
        {
            if (actual == null)
            {
                throw new ArgumentNullException(nameof(actual));
            }

            if (expected == null)
            {
                throw new ArgumentNullException(nameof(expected));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (actual.Type != expected.Type)
            {
                if (context.DefaultComparisonOptions.TreatJsonStringsAsObjects && actual.Type == JTokenType.String)
                {
                    var actualString = actual.Value<string>();
                    switch (expected.Type)
                    {
                        case JTokenType.Object:
                            if (actualString.StartsWith("{"))
                            {
                                var actualObject = (JToken)JsonConvert.DeserializeObject(actualString);
                                Compare(actualObject, expected, context);
                                return;
                            }

                            break;
                        case JTokenType.Array:
                            if (actualString.StartsWith("["))
                            {
                                var actualArray = (JToken)JsonConvert.DeserializeObject(actualString);
                                Compare(actualArray, expected, context);
                                return;
                            }

                            break;
                    }
                }

                context.AddDifference(new Difference { Message = $"Types do not match: expected {expected.Type} but found {actual.Type}" });
                return;
            }

            switch (expected.Type)
            {
                case JTokenType.Array:
                    CompareArrays((JArray)actual, (JArray)expected, context);
                    break;
                case JTokenType.Null:
                    break;
                case JTokenType.Object:
                    CompareObjects((JObject)actual, (JObject)expected, context);
                    break;
                default:
                    CompareValues(actual, expected, context);
                    break;
            }
        }

        private static void CompareValues(JToken actualValue, JToken expectedValue, IComparisonContext context)
        {
            var valuesAreEqual = (actualValue.Type == JTokenType.String && expectedValue.Type == JTokenType.String) 
                ? context.DefaultComparisonOptions.StringValueComparer.Equals(actualValue.Value<string>(), expectedValue.Value<string>())
                : actualValue.Value<object>().Equals(expectedValue.Value<object>());

            if (!valuesAreEqual)
            {
                context.AddDifference(new Difference { Message = $"{actualValue.Value<object>()} ({actualValue.Type}) does not equal {expectedValue.Value<object>()} ({expectedValue.Type})" });
            }
        }

        private void CompareObjects(JObject actual, JObject expected, IComparisonContext context)
        {
            var inlineOptionsPropertyName = context.DefaultComparisonOptions.InlineOptionsPropertyName;
            var comparisonOptions = string.IsNullOrEmpty(inlineOptionsPropertyName)
                ? null
                : expected.Property(inlineOptionsPropertyName)?.Value.ToObject<ComparisonOptions>();
            var comparisonOptionsToUse = comparisonOptions ?? context.DefaultComparisonOptions;

            foreach (var expectedProperty in expected.Properties().Where(p => p.Name != (inlineOptionsPropertyName ?? "")))
            {
                var actualProperty = actual.Property(expectedProperty.Name, comparisonOptionsToUse.PropertyNameComparer);
                using (var propertyContext = context.ForProperty(expectedProperty.Name, comparisonOptionsToUse))
                {

                    if (actualProperty == null)
                    {
                        propertyContext.AddDifference(new Difference
                        {
                            PropertyName = expectedProperty.Name,
                            Message = $"Property was not found, in the list of {string.Join(", ", actual.Properties().Select(p => p.Name))}"
                        });
                        continue;
                    }

                    Compare(actualProperty.Value, expectedProperty.Value, propertyContext);
                }
            }

            var actualPropertiesThatAreNotExpected = actual.Properties()
                .Where(p => expected.Property(p.Name, comparisonOptionsToUse.PropertyNameComparer) == null)
                .ToArray();

            if (actualPropertiesThatAreNotExpected.Any() && comparisonOptionsToUse.Explicit)
            {
                context.AddDifference(new Difference
                {
                    Message = $"Found {actualPropertiesThatAreNotExpected.Length} unexpected properties: {string.Join(", ", actualPropertiesThatAreNotExpected.Select(p => p.Name))}"
                });
            }
        }

        private void CompareArrays(JArray actual, JArray expected, IComparisonContext context)
        {
            var actualValues = actual.ToList();
            var index = 0;
            foreach (var expectedItem in expected)
            {
                using (var itemContext = context.ForItem(index))
                {
                    var actualItem = index < actualValues.Count ? actualValues[index] : null;

                    if (actualItem == null)
                    {
                        itemContext.AddDifference(new Difference { Message = "Item is missing" });
                    }
                    else
                    {
                        Compare(actualItem, expectedItem, itemContext);
                    }

                    index++;
                }
            }

            if (actualValues.Count > expected.Count)
            {
                context.AddDifference(new Difference { Message = $"There are more items {actualValues.Count} than expected {expected.Count}" });
            }
        }
    }
}