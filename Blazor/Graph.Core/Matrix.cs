
using System;
using System.Collections.Generic;

namespace Graph.Core
{
    public class Matrix
    {
        private readonly int weigthIfNoEdge = 0;
        private readonly int defaultWeigthIfHasEdge = 1;
        private readonly int maxEdgeWeigth = 10;

        private bool IsConsistent => DepthFirstSearch();

        private List<List<int>> elements = new List<List<int>>();

        public Matrix(int nodeNumber, int probabilityForEdge)
        {

        }

        public IEnumerable<int> GetNodeNeighbors()
        {
            throw new NotImplementedException();
        }


        private bool DepthFirstSearch()
        {
            throw new NotImplementedException();

        }
    }
}
