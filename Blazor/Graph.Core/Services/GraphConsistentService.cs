using Graph.Core.Models;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IGraphConsistentService
    {
        bool IsConsistent(int[][] elements);
        List<INodeNeighbors> GetNodeNeighbors(int[][] elements);
        List<INodeNeighbors> GetNodeNeighbors(IMatrix matrix);
    }

    public class GraphConsistentService : IGraphConsistentService
    {
        public bool IsConsistent(int[][] elements)
        {
            return DepthFirstSearch(elements);
        }

        public List<INodeNeighbors> GetNodeNeighbors(int[][] elements)
        {
            var result = new List<INodeNeighbors>();
            //Copy matrix by diagonal (DFS need two way graph for searching)
            for (var i = 0; i < elements.Length; i++)
            {
                for (var j = (i + 1); j < elements.Length; j++)
                {
                    elements[j][i] = elements[i][j];
                }
            }

            for (var i = 0; i < elements.Length; i++)
            {
                var tmp = new List<INodeNeighbor>();
                for (var j = 0; j < elements[i].Length; j++)
                {
                    if (elements[i][j] >= 1)
                    {
                        tmp.Add(new NodeNeighbor { NeighborNumber = j, EdgeValue = elements[i][j] });
                    }
                }
                tmp.Sort((a, b) => b.NeighborNumber - a.NeighborNumber);

                result.Add(new NodeNeighbors { Id = i, Neighbors = tmp.ToArray() });
            }
            return result;
        }

        public List<INodeNeighbors> GetNodeNeighbors(IMatrix matrix)
        {
            return GetNodeNeighbors(matrix.Elements);
        }

        private bool DepthFirstSearch(int[][] elements)
        {
            var nodeNeighbors = GetNodeNeighbors(elements);
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
            return visited.Count == elements.Length;
        }
    }
}
