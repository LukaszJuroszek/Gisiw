namespace Graph.Component.Models.CanvasJs.Options
{
    public interface ILegendOptions
    {
        string Cursor { get; set; }
        string VerticalAlign { get; set; }
        string HorizontalAlign { get; set; }
        int FontSize { get; set; }
        string FontColor { get; set; }
    }

    public class LegendOptions : ILegendOptions
    {
        public string Cursor { get; set; }
        public string VerticalAlign { get; set; }
        public string HorizontalAlign { get; set; }
        public int FontSize { get; set; }
        public string FontColor { get; set; }
    }
}