namespace Graph.Component.Models.CanvasJs.Data
{
    public class CanvasJsData
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string AxisYType { get; set; }
        public int AxisYIndex { get; set; }
        public bool ShowLegend { get; set; }
        public CanvasJSDataPoint[] DataPoints { get; set; }
    }
}
