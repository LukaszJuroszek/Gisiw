namespace Graph.Component.Models.Graph.Options
{
    public interface IGraphOptions
    {
        bool AutoResize { get; set; }
        bool ClickToUse { get; set; }
        EdgeOptions Edges { get; set; }
        string Height { get; set; }
        NodeOptions Nodes { get; set; }
        string Width { get; set; }
    }

    public class DefaultGraphOptions : IGraphOptions
    {
        public bool AutoResize { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public bool ClickToUse { get; set; }
        public EdgeOptions Edges { get; set; }
        public PhysicsOptions Physics { get; set; }
        public NodeOptions Nodes { get; set; }
    }

    public class BestChromosomeGraphOptions : IGraphOptions
    {
        public bool AutoResize { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public bool ClickToUse { get; set; }
        public EdgeOptions Edges { get; set; }
        public NodeOptions Nodes { get; set; }
        public LayoutOptions Layout { get; set; }
    }
}
