using Graph.Component.Models.Data;
using Graph.Core.Models;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IGraphChartService
    {
        GraphData GraphDataFromMatrix(MatrixModel matrix);
        IList<GraphEdges> GraphEdgesFromMatrix(MatrixModel matrix);
        IList<GraphNodes> GraphNodesFromMatrix(MatrixModel matrix);
    }

    public class GraphChartService : IGraphChartService
    {
        private readonly IMatrixService _matrixService;

        public GraphChartService(IMatrixService matrixService)
        {
            _matrixService = matrixService;
        }

        public GraphData GraphDataFromMatrix(MatrixModel matrix)
        {
            return new GraphData
            {
                Edges = GraphEdgesFromMatrix(matrix),
                Nodes = GraphNodesFromMatrix(matrix)
            };
        }

        public IList<GraphEdges> GraphEdgesFromMatrix(MatrixModel matrix)
        {
            var nodeNeighbors = _matrixService.GetNodeNeighbors(matrix);

            var result = new List<GraphEdges>();

            for (var i = 0; i < nodeNeighbors.Count; i++)
            {
                for (var j = 0; j < nodeNeighbors[i].Neighbors.Count; j++)
                {
                    result.Add(new GraphEdges
                    {
                        From = nodeNeighbors[i].Id.ToString(),
                        To = nodeNeighbors[i].Neighbors[j].NeighborNumber.ToString(),
                        Label = nodeNeighbors[i].Neighbors[j].EdgeValue.ToString(),
                        Font = new Font { Align = "top" }
                    });
                    result.Add(new GraphEdges
                    {
                        From = nodeNeighbors[i].Neighbors[j].NeighborNumber.ToString(),
                        To = nodeNeighbors[i].Id.ToString(),
                    });
                }
            }
            return result;
        }

        public IList<GraphNodes> GraphNodesFromMatrix(MatrixModel matrix)
        {
            var result = new List<GraphNodes>();

            for (var i = 0; i < matrix.Elements.Length; i++)
            {
                result.Add(new GraphNodes { Id = i, Label = i.ToString() });
            }

            return result;
        }
    }
}
