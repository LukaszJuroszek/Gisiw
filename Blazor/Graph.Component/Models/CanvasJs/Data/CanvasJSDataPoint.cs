namespace Graph.Component.Models.CanvasJs.Data
{
    public interface ICanvasJSDataPoint
    {
        string Color { get; set; }
        int X { get; set; }
        int Y { get; set; }
    }

    public class CanvasJSDataPoint : ICanvasJSDataPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; }
    }
}