using Newtonsoft.Json.Linq;

namespace JsonEqualityComparer
{
    public interface IJsonEqualityComparer
    {
        IComparisonContext Compare(JToken actual, JToken expected);
        void Compare(JToken actual, JToken expected, IComparisonContext context);
    }
}