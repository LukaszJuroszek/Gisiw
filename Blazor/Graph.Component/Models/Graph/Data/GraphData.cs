using System.Collections.Generic;

namespace Graph.Component.Models.Graph.Data
{
    public interface IGraphData
    {
        IEnumerable<GraphEdges> Edges { get; set; }
        IEnumerable<GraphNodes> Nodes { get; set; }
    }

    public class GraphData : IGraphData
    {
        public IEnumerable<GraphNodes> Nodes { get; set; }
        public IEnumerable<GraphEdges> Edges { get; set; }
    }
}
