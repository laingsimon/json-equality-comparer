namespace JsonEqualityComparer
{
    public class ComparisonOptions
    {
        /// <summary>
        /// Perform strict JSON object comparison. If strict then any property in actual JSON must be mentioned in expected JSON
        /// Otherwise properties can exist in the actual JSON without being present in the expected JSON
        /// 
        /// Either way, if the property is in both then the values must match
        /// </summary>
        public bool Explicit { get; set; }

        /// <summary>
        /// If a property exists in the expected JSON object as an object and as a string in the actual JSON object
        /// Should the comparison process treat them as identical if the actual (string) property contains a JSON string
        /// </summary>
        public bool TreatJsonStringsAsObjects { get; set; }

        /// <summary>
        /// The name of the property that can contain inline ComparisonOptions overrides
        /// Set this property to null or an empty string to disable the feature
        /// </summary>
        public string InlineOptionsPropertyName { get; set; } = "$ComparisonOptions";
    }
}