using Graph.Component.Models.CanvasJs.Options;

namespace Graph.Component.Models.CanvasJs
{
    public class CanvasJsConfig
    {
        public bool AnimationEnabled { get; set; }
        public bool ZoomEnabled { get; set; }
        public bool ExportEnabled { get; set; }
        public IAxisOptions AxisY { get; set; }
        public IAxisOptions[] AxisY2 { get; set; }
        public ILegendOptions Legend { get; set; }
        public bool ToolTip { get; set; }
        public string Theme { get; set; }
    }
}
