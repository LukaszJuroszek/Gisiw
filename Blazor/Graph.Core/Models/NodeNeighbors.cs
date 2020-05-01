namespace Graph.Core.Models
{
    public interface INodeNeighbors
    {
        int Id { get; set; }
        INodeNeighbor[] Neighbors { get; set; }
    }

    public class NodeNeighbors : INodeNeighbors
    {
        public int Id { get; set; }
        public INodeNeighbor[] Neighbors { get; set; }
    }

    public interface INodeNeighbor
    {
        int EdgeValue { get; set; }
        int NeighborNumber { get; set; }
    }

    public class NodeNeighbor : INodeNeighbor
    {
        public int NeighborNumber { get; set; }
        public int EdgeValue { get; set; }
    }
}