using Graph.Component.Models.CanvasJs.Data;
using Graph.Component.Models.CanvasJs.Options;
using System.Collections.Generic;

namespace Graph.Component.Models.CanvasJs
{
    public interface ICanvasJsConfig
    {
        bool AnimationEnabled { get; set; }
        IAxisOptions AxisY { get; set; }
        IAxisOptions[] AxisY2 { get; set; }
        IEnumerable<ICanvasJsData> Data { get; set; }
        bool ExportEnabled { get; set; }
        ILegendOptions Legend { get; set; }
        string Theme { get; set; }
        IToolTipOptions ToolTip { get; set; }
        bool ZoomEnabled { get; set; }
    }

    public class CanvasJsConfig : ICanvasJsConfig
    {
        public bool AnimationEnabled { get; set; }
        public bool ZoomEnabled { get; set; }
        public bool ExportEnabled { get; set; }
        public IAxisOptions AxisY { get; set; }
        public IAxisOptions[] AxisY2 { get; set; }
        public ILegendOptions Legend { get; set; }
        public IToolTipOptions ToolTip { get; set; }
        public string Theme { get; set; } = string.Empty;
        public IEnumerable<ICanvasJsData> Data { get; set; }
    }
}
