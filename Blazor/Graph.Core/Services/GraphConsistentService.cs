using Graph.Core.Models;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IGraphConsistentService
    {
        bool IsConsistent(MatrixModel matrix);
    }

    public class GraphConsistentService : IGraphConsistentService
    {
        private readonly IMatrixService _matrixService;

        public GraphConsistentService(IMatrixService matrixService)
        {
            _matrixService = matrixService;
        }

        public bool IsConsistent(MatrixModel matrix)
        {
            return DepthFirstSearch(matrix);
        }

        private bool DepthFirstSearch(MatrixModel matrix)
        {
            var nodeNeighbors = _matrixService.GetNodeNeighbors(matrix);
            var stack = new Stack<int>();
            var visited = new HashSet<int>();
            stack.Push(0);
            while (stack.Count > 0)
            {
                var vertex = stack.Pop();
                visited.Add(vertex);
                var node = nodeNeighbors.Find(n => n.Id == vertex);
                foreach (var neighbor in node.Neighbors)
                {
                    if (visited.Contains(neighbor.NeighborNumber) == false)
                    {
                        stack.Push(neighbor.NeighborNumber);
                    }
                }
            }
            return visited.Count == matrix.Elements.Length;
        }
    }
}
