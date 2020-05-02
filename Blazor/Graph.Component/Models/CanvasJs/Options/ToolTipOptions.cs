namespace Graph.Component.Models.CanvasJs.Options
{
    public interface IToolTipOptions
    {
        bool Shared { get; set; }
    }

    public class ToolTipOptions : IToolTipOptions
    {
        public bool Shared { get; set; }
    }
}
