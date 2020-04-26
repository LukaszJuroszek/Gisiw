using System.Collections.Generic;

namespace Graph.Core.Services
{
    public class NodeNeighborsModel
    {
        public int Id { get; set; }
        public List<NodeNeighborModel> Neighbors { get; set; }
    }

    public class NodeNeighborModel
    {
        public int NeighborNumber { get; set; }
        public int EdgeValue { get; set; }
    }
}