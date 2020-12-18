using System;
using System.Collections.Generic;

namespace JsonEqualityComparer
{
    public interface IComparisonContext : IDisposable
    {
        ComparisonOptions DefaultComparisonOptions { get; set; }
        List<Difference> Differences { get; }
        string Path { get; }
        IComparisonContext RootContext { get; }

        void AddDifference(Difference difference);
        IComparisonContext ForItem(int index, ComparisonOptions comparisonOptions = null);
        IComparisonContext ForProperty(string property, ComparisonOptions comparisonOptions = null);
        string ToString();
    }
}