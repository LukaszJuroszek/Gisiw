using System.Collections.Generic;

namespace Graph.Component.Models.CanvasJs.Data
{
    public interface ICanvasJsData
    {
        int AxisYIndex { get; set; }
        string AxisYType { get; set; }
        List<ICanvasJSDataPoint> DataPoints { get; set; }
        string Name { get; set; }
        bool ShowInLegend { get; set; }
        string Type { get; set; }
    }

    public class CanvasJsData : ICanvasJsData
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string AxisYType { get; set; } = "primary";
        public int AxisYIndex { get; set; }
        public bool ShowInLegend { get; set; }
        public List<ICanvasJSDataPoint> DataPoints { get; set; }
    }
}
