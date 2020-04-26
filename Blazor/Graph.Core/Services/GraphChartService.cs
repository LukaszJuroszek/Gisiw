using Graph.Component.Models.Data;
using Graph.Component.Models.Options;
using Graph.Core.Models;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IGraphChartService
    {
        GraphData GraphDataFromMatrix(MatrixModel matrix);
        IList<GraphEdges> GraphEdgesFromMatrix(MatrixModel matrix);
        IList<GraphNodes> GraphNodesFromMatrix(MatrixModel matrix);
        GraphOptions GetDefaultGraphOptions();
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

        public GraphOptions GetDefaultGraphOptions()
        {
            return new GraphOptions
            {
                AutoResize = true,
                Height = "100%",
                Width = "100%",
                ClickToUse = false,
                Edges = new EdgeOptions
                {
                    Color = new ColorOption { Color = "3E89D" },
                    Shadow = false,
                    Smooth = new SmoothOption
                    {
                        Type = "vertical",
                        ForceDirection = "none",
                        Roundness = 0d,
                        Enabled = true
                    }
                },
                Physics = new PhysicsOptions
                {
                    ForceAtlas2Based = new ForceAtlas2Based
                    {
                        GravitationalConstant = -30,
                        CentralGravity = 0.005d,
                        SpringLength = 135,
                        SpringConstant = 0,
                        Damping = 1
                    },
                    MaxVelocity = 40,
                    MinVelocity = 0.3d,
                    Solver = "forceAtlas2Based",
                    Timestep = 1
                },
                Nodes = new NodeOptions
                {
                    BorderWidth = 1,
                    BorderWidthSelected = 2,
                    Chosen = true,
                    Color = new NodeColorOption
                    {
                        Border = "green",
                        Background = "withe",
                        Highlight = new HighlightOption
                        {
                            Background = "#D2E5FF",
                            Border = "2B7CE9"
                        },
                        Hover = new HoverOption
                        {
                            Background = "#D2E5FF",
                            Border = "2B7CE9"
                        }
                    }
                }
            };
        }
    }
}
