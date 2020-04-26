namespace Graph.Component.Models.Data
{
    public class GraphEdges
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Label { get; set; }
        public Font Font { get; set; }
    }

    public class Font
    {
        public string Align { get; set; } = "top";
    }
}