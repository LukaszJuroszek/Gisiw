using Graph.Component.Models.Data;
using Graph.Component.Models.Options;
using Graph.Core.Models;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IGraphChartService
    {
        GraphData GraphDataFromMatrix(IMatrix matrix);
        IList<GraphEdges> GraphEdgesFromMatrix(IMatrix matrix);
        IList<GraphNodes> GraphNodesFromMatrix(IMatrix matrix);
        GraphOptions GetDefaultGraphOptions();
    }

    public class GraphChartService : IGraphChartService
    {
        private readonly IGraphConsistentService _graphConsistentService;

        public GraphChartService(IGraphConsistentService graphConsistentService)
        {
            _graphConsistentService = graphConsistentService;
        }

        public GraphData GraphDataFromMatrix(IMatrix matrix)
        {
            return new GraphData
            {
                Edges = GraphEdgesFromMatrix(matrix),
                Nodes = GraphNodesFromMatrix(matrix)
            };
        }

        public IList<GraphEdges> GraphEdgesFromMatrix(IMatrix matrix)
        {
            var nodeNeighbors = _graphConsistentService.GetNodeNeighbors(matrix);

            var result = new List<GraphEdges>();

            for (var i = 0; i < nodeNeighbors.Count; i++)
            {
                for (var j = 0; j < nodeNeighbors[i].Neighbors.Length; j++)
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

        public IList<GraphNodes> GraphNodesFromMatrix(IMatrix matrix)
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
                        GravitationalConstant = -50,
                        CentralGravity = 0.01d,
                        SpringLength = 50,
                        SpringConstant = 0d,
                        Damping = 0.4d
                    },
                    MaxVelocity = 50,
                    MinVelocity = 0.7d,
                    Solver = "forceAtlas2Based",
                    Timestep = 1d,
                    Stabilization = new Stabilization
                    {
                        Enabled = true,
                        Iterations = 200,
                        UpdateInterval = 25
                    }
                },
                Nodes = new NodeOptions
                {
                    Shape = "circle",
                    BorderWidth = 1,
                    BorderWidthSelected = 2,
                    Chosen = true,
                    Color = new NodeColorOption
                    {
                        Border = "green",
                        Background = "white",
                        Highlight = new HighlightOption
                        {
                            Background = "#D2E5FF",
                            Border = "#2B7CE9"
                        },
                        Hover = new HoverOption
                        {
                            Background = "#D2E5FF",
                            Border = "#2B7CE9"
                        }
                    }
                }
            };
        }
    }
}
