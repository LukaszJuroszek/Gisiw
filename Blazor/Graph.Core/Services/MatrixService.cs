using Graph.Core.Models;
using System;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IMatrixService
    {
        IEnumerable<int> GetNodeNeighbors();
        bool DepthFirstSearch();
        MatrixModel GenerateMatrix(int nodeCount, double probabilityForEdge);
    }

    public class MatrixService : IMatrixService
    {
        public IEnumerable<int> GetNodeNeighbors()
        {
            throw new NotImplementedException();
        }

        public bool DepthFirstSearch()
        {
            throw new NotImplementedException();
        }

        public MatrixModel GenerateMatrix(int nodeCount, double probabilityForEdge)
        {
            throw new NotImplementedException();
        }
    }
}
