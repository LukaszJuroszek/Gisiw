namespace Graph.Component.Models.Graph.Options
{
    public class NodeOptions
    {
        public double BorderWidth { get; set; }
        public double BorderWidthSelected { get; set; }
        public bool Chosen { get; set; }
        public string Shape { get; set; }
        public NodeColorOption Color { get; set; }
    }

    public class NodeColorOption
    {
        public string Border { get; set; }
        public string Background { get; set; }
        public HighlightOption Highlight { get; set; }
        public HoverOption Hover { get; set; }
    }

    public class HighlightOption
    {
        public string Border { get; set; }
        public string Background { get; set; }
    }

    public class HoverOption
    {
        public string Border { get; set; }
        public string Background { get; set; }
    }
}