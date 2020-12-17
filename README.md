# json-comparer

Compare JSON objects simply and flexibly. Report the differences programmatically.

### Supports:
- Strict and flexible comparison (see below)
- Per-section comparison options
- Comparison of JSON-strings to JSON-objects

### Strict vs flexible comparison
Strict comparison is well known. Every property in each JSON object must exist in the other.

Flexible comparison is where only the properties in the expected JSON object must exist in the actual JSON object.

## Example

```
var comparer = new JsonEqualityComparer();
var context = new ComparisonContext();

comparer.Compare(expected, actual, context);

var differences = context.Differences;
```

If you need to vary the comparison options, either:
```
var comparer = new JsonEqualityComparer();
var context = new ComparisonContext
{
  DefaultComparisonOptions = new ObjectMatchOptions
  {
    Explicit = true
  }
};

comparer.Compare(expected, actual, context);

var differences = context.Differences;
```
