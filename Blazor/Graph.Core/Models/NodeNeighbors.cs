namespace Graph.Core.Models
{
    public interface INodeNeighbors : IDeepCopy<INodeNeighbors>
    {
        int Id { get; }
        INodeNeighbor[] Neighbors { get; }
    }

    public class NodeNeighbors : INodeNeighbors
    {
        public int Id { get; }
        public INodeNeighbor[] Neighbors { get; }

        public NodeNeighbors(int id, INodeNeighbor[] neighbors)
        {
            Id = id;
            Neighbors = neighbors;
        }

        public INodeNeighbors DeepCopy()
        {
            return new NodeNeighbors(id: Id, neighbors: (INodeNeighbor[])Neighbors.Clone());
        }
    }

    public interface INodeNeighbor : IDeepCopy<INodeNeighbor>
    {
        int EdgeValue { get; }
        int NeighborNumber { get; }
    }

    public class NodeNeighbor : INodeNeighbor
    {
        public int NeighborNumber { get; }
        public int EdgeValue { get; }

        public NodeNeighbor(int neighborNumber, int edgeValue)
        {
            NeighborNumber = neighborNumber;
            EdgeValue = edgeValue;
        }

        public INodeNeighbor DeepCopy() => new NodeNeighbor(neighborNumber: NeighborNumber, edgeValue: EdgeValue);
    }
}