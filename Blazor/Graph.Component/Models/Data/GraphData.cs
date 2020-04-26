using System.Collections.Generic;

namespace Graph.Component.Models.Data
{
    public class GraphData
    {
        public IEnumerable<GraphNodes> Nodes { get; set; }
        public IEnumerable<GraphEdges> Edges { get; set; }
    }
}
