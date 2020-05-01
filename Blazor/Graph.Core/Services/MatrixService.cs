using Graph.Core.Models;
using Graph.Core.Utils;
using System;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IMatrixService
    {
        IMatrix GenerateMatrix(int nodeCount, double probability);
    }

    public class MatrixService : IMatrixService
    {
        private readonly IGraphConsistentService _graphConsistentService;
        private readonly int _weigthIfHasNotEdge = 0;
        private readonly int _weigthIfHasEdge = 1;
        private readonly int _maxEdgeWeigth = 10;

        public MatrixService(IGraphConsistentService graphConsistentService)
        {
            _graphConsistentService = graphConsistentService;
        }

        public IEnumerable<int> GetNodeNeighbors()
        {
            throw new NotImplementedException();
        }

        public IMatrix GenerateMatrix(int nodeCount, double probability)
        {
            var tryCount = 0;
            int[][] result;
            var random = new Random();
            do
            {
                result = new int[nodeCount][];
                probability = ProbabilityUtils.AdaptProbability(probability, tryCount);

                for (var i = 0; i < nodeCount; i++)
                {
                    result[i] = new int[nodeCount];
                    for (var j = 0; j < nodeCount; j++)
                    {
                        result[i][j] = _weigthIfHasNotEdge;
                    }
                }
                for (var i = 0; i < nodeCount; i++)
                {
                    for (var j = (i + 1); j < nodeCount; j++)
                    {
                        var hasEdge = random.NextDouble() < probability;
                        result[i][j] = hasEdge ? (int)Math.Floor((random.NextDouble() * _maxEdgeWeigth) + _weigthIfHasEdge) : _weigthIfHasNotEdge;
                        result[j][i] = _weigthIfHasNotEdge;
                    }
                }

                tryCount++;

            } while (_graphConsistentService.IsConsistent(result) == false);

            return new Matrix(result);
        }
    }
}
