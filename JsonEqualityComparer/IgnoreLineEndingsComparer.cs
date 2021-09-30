using System.Collections.Generic;

namespace JsonEqualityComparer
{
    public class IgnoreLineEndingsComparer : IEqualityComparer<string>
    {
        private readonly IEqualityComparer<string> underlyingComparer;

        public IgnoreLineEndingsComparer(IEqualityComparer<string> underlyingComparer = null)
        {
            this.underlyingComparer = underlyingComparer ?? EqualityComparer<string>.Default;
        }

        public bool Equals(string x, string y)
        {
            return underlyingComparer.Equals(NormaliseLineEndings(x), NormaliseLineEndings(y));
        }

        public int GetHashCode(string obj)
        {
            return NormaliseLineEndings(obj).GetHashCode();
        }

        private static string NormaliseLineEndings(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Replace("\r\n", "\n");
        }
    }
}
