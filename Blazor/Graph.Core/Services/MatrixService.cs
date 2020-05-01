using Graph.Core.Models;
using System;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IMatrixService
    {
        MatrixModel GenerateMatrix(int nodeCount, double edgeProbability);
        List<NodeNeighborsModel> GetNodeNeighbors(MatrixModel matrix);
    }

    public class MatrixService : IMatrixService
    {
        private readonly int weigthIfHasNotEdge = 0;
        private readonly int weigthIfHasEdge = 1;
        private readonly int maxEdgeWeigth = 10;

        public IEnumerable<int> GetNodeNeighbors()
        {
            throw new NotImplementedException();
        }

        public MatrixModel GenerateMatrix(int nodeCount, double edgeProbability)
        {
            var result = new int[nodeCount][];  
            for (var i = 0; i < nodeCount; i++)
            {
                result[i] = new int[nodeCount];
                for (var j = 0; j < nodeCount; j++)
                {
                    result[i][j] = weigthIfHasNotEdge;
                }
            }
            var random = new Random();
            for (var i = 0; i < nodeCount; i++)
            {
                for (var j = (i + 1); j < nodeCount; j++)
                {
                    var hasEdge = random.NextDouble() < edgeProbability;
                    result[i][j] = hasEdge ? (int)Math.Floor((random.NextDouble() * maxEdgeWeigth) + weigthIfHasEdge) : weigthIfHasNotEdge;
                    result[j][i] = weigthIfHasNotEdge;
                }
            }

            return new MatrixModel(result);
        }

        public List<NodeNeighborsModel> GetNodeNeighbors(MatrixModel matrix)
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
