using Graph.Component.Models.Graph.Data;
using Graph.Component.Models.Graph.Options;
using Graph.Core.Models;
using StackExchange.Profiling;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IGraphChartService
    {
        BestChromosomeGraphOptions GetBestChromosomeGraphOptions();
        DefaultGraphOptions GetDefaultGraphOptions();
        GraphData GraphDataFromChromosome(IChromosome chromosome, IMatrix matrix);
        GraphData GraphDataFromMatrix(IMatrix matrix);
        IList<GraphEdges> GraphEdges(IMatrix matrix);
        IList<GraphNodes> GraphNodes(IChromosome chromosome);
        IList<GraphNodes> GraphNodes(IMatrix matrix);
    }

    public class GraphChartService : IGraphChartService
    {
        private readonly IGraphConsistentService _graphConsistentService;
        private readonly MiniProfiler _profiler;

        public GraphChartService(IGraphConsistentService graphConsistentService)
        {
            _graphConsistentService = graphConsistentService;
            _profiler = MiniProfiler.StartNew(nameof(GraphChartService));
        }

        public GraphData GraphDataFromMatrix(IMatrix matrix)
        {
            return new GraphData
            {
                Edges = GraphEdges(matrix),
                Nodes = GraphNodes(matrix)
            };
        }

        public IList<GraphEdges> GraphEdges(IMatrix matrix)
        {
            var nodeNeighbors = _graphConsistentService.GetNodeNeighbors(matrix);

            var result = new List<GraphEdges>();
            using (_profiler.Step(nameof(GraphEdges)))
            {
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
            }
            return result;
        }

        public IList<GraphNodes> GraphNodes(IMatrix matrix)
        {
            var result = new List<GraphNodes>();

            for (var i = 0; i < matrix.Elements.Length; i++)
            {
                result.Add(new GraphNodes { Id = i, Label = i.ToString() });
            }

            return result;
        }

        public GraphData GraphDataFromChromosome(IChromosome chromosome, IMatrix matrix)
        {
            return new GraphData
            {
                Edges = GraphEdges(matrix),
                Nodes = GraphNodes(chromosome)
            };
        }

        public IList<GraphNodes> GraphNodes(IChromosome chromosome)
        {
            var result = new List<GraphNodes>();
            for (var i = 0; i < chromosome.Distribution.Count; i++)
            {
                result.Add(new GraphNodes { Id = i, Label = i.ToString(), Level = Chromosome.MapToLevel(chromosome.Distribution[i]) });
            }

            return result;
        }

        public BestChromosomeGraphOptions GetBestChromosomeGraphOptions()
        {
            return new BestChromosomeGraphOptions
            {
                Layout = new LayoutOptions
                {
                    Hierarchical = new HierarchicalOption
                    {
                        Enabled = true,
                        LevelSeparation = 500,
                        BlockShifting = false,
                        ParentCentralization = false,
                        Direction = "DU",
                        SortMethod = "directed"
                    }
                },
                AutoResize = true,
                Height = "100%",
                Width = "100%",
                ClickToUse = false,
                Edges = new EdgeOptions
                {
                    Color = new ColorOption { Color = "3E89D5" },
                    Shadow = false,
                    Smooth = new SmoothOption
                    {
                        Type = "vertical",
                        ForceDirection = "none",
                        Roundness = 0d,
                        Enabled = true
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
                            Border = "#2B7CE9",
                            Background = "#D2E5FF"
                        },
                        Hover = new HoverOption
                        {
                            Border = "#2B7CE9",
                            Background = "#D2E5FF"
                        }
                    }
                },
            };
        }

        public DefaultGraphOptions GetDefaultGraphOptions()
        {
            return new DefaultGraphOptions
            {
                AutoResize = true,
                Height = "100%",
                Width = "100%",
                ClickToUse = false,
                Edges = new EdgeOptions
                {
                    Color = new ColorOption { Color = "3E89D5" },
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
                        Iterations = 50,
                        UpdateInterval = 10
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
