using Graph.Core.Models;
using System;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IMatrixService
    {
        IEnumerable<int> GetNodeNeighbors();
        MatrixModel GenerateMatrix(int nodeCount, double edgeProbability);
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
    }
}
