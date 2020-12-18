using System.Collections.Generic;

namespace JsonEqualityComparer
{
    public class ComparisonContext : IComparisonContext
    {
        /// <summary>
        /// The current location within the JSON object
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// A reference to the root context of the comparison operation
        /// </summary>
        public IComparisonContext RootContext { get; }

        /// <summary>
        /// A list of all the differences between the two JSON objects
        /// </summary>
        public List<Difference> Differences { get; }

        /// <summary>
        /// The options to use when comparing the JSON object
        /// </summary>
        public ComparisonOptions DefaultComparisonOptions { get; set; } = new ComparisonOptions();

        private ComparisonContext(IComparisonContext rootContext, string path)
        {
            RootContext = rootContext;
            Differences = new List<Difference>();
            Path = path;
        }

        public ComparisonContext()
        {
            Differences = new List<Difference>();
            Path = "";
            RootContext = this;
        }

        public IComparisonContext ForProperty(string property, ComparisonOptions comparisonOptions = null)
        {
            var prefix = string.IsNullOrEmpty(Path) ? "" : $"{Path}.";
            return new ComparisonContext(RootContext, $"{prefix}{property}")
            {
                DefaultComparisonOptions = comparisonOptions ?? DefaultComparisonOptions
            };
        }

        public IComparisonContext ForItem(int index, ComparisonOptions comparisonOptions = null)
        {
            var prefix = string.IsNullOrEmpty(Path) ? "" : $"{Path}";
            return new ComparisonContext(RootContext, $"{prefix}[{index}]")
            {
                DefaultComparisonOptions = comparisonOptions ?? DefaultComparisonOptions
            };
        }

        public void AddDifference(Difference difference)
        {
            difference.PropertyPath = Path;
            Differences.Add(difference);
        }

        public override string ToString()
        {
            return Path;
        }

        public void Dispose()
        {
            if (RootContext != this)
                RootContext.Differences.AddRange(Differences);
        }
    }
}