# JsonEqualityComparer

[![Build status](https://ci.appveyor.com/api/projects/status/8h86p0iegkj1aiuh/branch/main?svg=true)](https://ci.appveyor.com/project/laingsimon/json-comparer/branch/main)

Compare JSON objects simply and flexibly. Report the differences programmatically.

### Supports:
- Strict and flexible comparison (see below)
- Per-section comparison options
- Comparison of JSON-strings to JSON-objects

#### Compatibility
This package is built using dotnet core 3.1, so should be compatible with dotnet core 3.1+ projects.

### Strict vs flexible comparison
Strict comparison is well known. Every property in each JSON object must exist in the other.

Flexible comparison is where only the properties in the expected JSON object must exist in the actual JSON object.

## Usage
Install the package from [nuget](https://www.nuget.org/packages/JsonEqualityComparer/)

```
Install-Package JsonEqualityComparer

or

dotnet add package JsonEqualityComparer
```

## Examples

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
  DefaultComparisonOptions =
  {
    Explicit = true
  }
};

comparer.Compare(expected, actual, context);

var differences = context.Differences;
```

Or specify the options in the expected JSON, e.g.

```
{
  "$ComparisonOptions": { "Explicit": true },
  ...
  {
    ...
    "$ComparisonOptions": { "Explicit": false }
  }
}
```

## Options

1. **Explicit** - configure whether both objects have to be identical
1. **TreatJsonStringsAsObjects** - configure whether JSON-strings can match expected JSON-objects
1. **InlineOptionsPropertyName** - configure the name of the inline comparison-options property in the expected JSON object
