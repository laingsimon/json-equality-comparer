namespace JsonEqualityComparer
{
    public class Difference
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
        public string PropertyPath { get; set; }

        public override string ToString()
        {
            return $"{PropertyPath}: {Message}";
        }
    }
}
