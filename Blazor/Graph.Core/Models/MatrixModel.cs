using System;
using System.Collections.Generic;

namespace Graph.Core.Models
{
    public class MatrixModel
    {
        private readonly int weigthIfNoEdge = 0;
        private readonly int defaultWeigthIfHasEdge = 1;
        private readonly int maxEdgeWeigth = 10;

        //private bool IsConsistent => DepthFirstSearch();

        private List<List<int>> elements = new List<List<int>>();

        public MatrixModel(int nodeNumber, int probabilityForEdge)
        {

        }
    }
}
