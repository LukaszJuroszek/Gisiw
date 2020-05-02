namespace Graph.Component.Models.CanvasJs.Data
{
    public interface ICanvasJsData
    {
        int AxisYIndex { get; set; }
        string AxisYType { get; set; }
        ICanvasJSDataPoint[] DataPoints { get; set; }
        string Name { get; set; }
        bool ShowLegend { get; set; }
        string Type { get; set; }
    }

    public class CanvasJsData : ICanvasJsData
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string AxisYType { get; set; }
        public int AxisYIndex { get; set; }
        public bool ShowLegend { get; set; }
        public ICanvasJSDataPoint[] DataPoints { get; set; }
    }
}
