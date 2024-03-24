namespace ChartsProject.Models
{
    public class SimpleChart
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public record Source
    {
        public string Owner { get; init; }
        public string Repo { get; init; }
    }
}
