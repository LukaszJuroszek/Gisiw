using System.Text.Json.Serialization;

namespace Graph.Component.Models.Graph.Options
{
    public class LayoutOptions
    {
        public HierarchicalOption Hierarchical { get; set; }
    }

    public class HierarchicalOption
    {
        public bool Enabled { get; set; }
        public int LevelSeparation { get; set; }
        public bool BlockShifting { get; set; }
        public bool ParentCentralization { get; set; }
        public string Direction { get; set; }
        public string SortMethod { get; set; }
    }
}