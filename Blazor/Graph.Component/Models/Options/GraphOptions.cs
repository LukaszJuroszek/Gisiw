namespace Graph.Component.Models.Options
{
    public class GraphOptions
    {
        public bool AutoResize { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public bool ClickToUse { get; set; }
        public EdgeOptions Edges { get; set; }
        public PhysicsOptions Physics { get; set; }
        public NodeOptions Nodes { get; set; }
    }
}
