namespace Graph.Component.Models.CanvasJs.Options
{
    public interface IAxisOptions
    {
        bool IncludeZero { get; set; }
        string GridColor { get; set; }
        int GridThickness { get; set; }
        int Interval { get; set; }
    }

    public class AxisOptions : IAxisOptions
    {
        public bool IncludeZero { get; set; }
        public string GridColor { get; set; }
        public int GridThickness { get; set; }
        public int Interval { get; set; }
    }
}