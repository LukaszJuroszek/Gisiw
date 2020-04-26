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
        public bool IsConsistent(MatrixModel matrix)
        {
            return DepthFirstSearch(matrix);
        }

        private bool DepthFirstSearch(MatrixModel matrix)
        {
            var nodeNeighbors = GetNodeNeighbors(matrix);
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

        private List<NodeNeighborsModel> GetNodeNeighbors(MatrixModel matrix)
        {
            var result = new List<NodeNeighborsModel>();
            //Copy matrix by diagonal (DFS need two way graph for searching)
            for (var i = 0; i < matrix.Elements.Length; i++)
            {
                for (var j = (i + 1); j < matrix.Elements.Length; j++)
                {
                    matrix.Elements[j][i] = matrix.Elements[i][j];
                }
            }

            for (var i = 0; i < matrix.Elements.Length; i++)
            {
                var tmp = new List<NodeNeighborModel>();
                for (var j = 0; j < matrix.Elements[i].Length; j++)
                {
                    if (matrix.Elements[i][j] >= 1)
                    {
                        tmp.Add(new NodeNeighborModel { NeighborNumber = j, EdgeValue = matrix.Elements[i][j] });
                    }
                }
                tmp.Sort((a, b) => b.NeighborNumber - a.NeighborNumber);

                result.Add(new NodeNeighborsModel { Id = i, Neighbors = tmp });
            }
            return result;
        }
    }
}
