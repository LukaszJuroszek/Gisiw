using Graph.Component.Models.CanvasJs;
using Graph.Component.Models.CanvasJs.Data;
using Graph.Component.Models.CanvasJs.Options;
using Graph.Core.Models;
using StackExchange.Profiling;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface ICanvasJsChartService
    {
        IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> AddToDataPoints(IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> dataPoints,
            IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> toAdd);
        ICanvasJsConfig GetBasicOptionsForEvolutionChart();
        IEnumerable<ICanvasJsData> GetEvolutionChartData(IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> dataPoints,
            List<ICanvasJSDataPoint> bestChromosomeDataPoints);
        IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> MapPopulationToDataPoints(IPopulationResult populationResult);
        ICanvasJSDataPoint MapToDataPoint(int bestFactorSum, int iteration);
    }

    public class CanvasJsChartService : ICanvasJsChartService
    {
        private readonly MiniProfiler _profiler;

        public CanvasJsChartService()
        {
            _profiler = MiniProfiler.StartNew(nameof(CanvasJsChartService));
        }

        public ICanvasJsConfig GetBasicOptionsForEvolutionChart()
        {
            return new CanvasJsConfig
            {
                AnimationEnabled = true,
                ZoomEnabled = true,
                ExportEnabled = true,

                AxisY = new IAxisOptions[]
                {
                    new AxisOptions
                    {
                        IncludeZero = false,
                        Title = "Best Chromosome",
                        GridColor = "black",
                        GridThickness = 1,
                    },
                },
                AxisY2 = new IAxisOptions[]
                {
                    new AxisOptions
                    {
                        IncludeZero = false,
                        Title = "Edge Count",
                        GridColor = "black",
                    },
                     new AxisOptions
                    {
                        IncludeZero = false,
                        Title = "Connected Edge",
                        GridColor = "black",
                    },
                },
                Legend = new LegendOptions
                {
                    Cursor = "pointer",
                    VerticalAlign = "top",
                    HorizontalAlign = "center",
                    FontSize = 18,
                    FontColor = "black",
                },
                ToolTip = new ToolTipOptions
                {
                    Shared = false
                }
            };
        }

        public IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> MapPopulationToDataPoints(IPopulationResult populationResult)
        {
            return new Dictionary<ChromosomeFactor, List<ICanvasJSDataPoint>>()
            {
                [ChromosomeFactor.EdgeCount] = new List<ICanvasJSDataPoint>
                {
                    MapToCanvasJsDataPoint(populationResult, ChromosomeFactor.EdgeCount)
                },
                [ChromosomeFactor.ConnectedEdgeWeigthSum] = new List<ICanvasJSDataPoint>
                {
                    MapToCanvasJsDataPoint(populationResult, ChromosomeFactor.ConnectedEdgeWeigthSum)
                },
                [ChromosomeFactor.ConnectedEdgeWeigthSum | ChromosomeFactor.EdgeCount] = new List<ICanvasJSDataPoint>
                {
                    MapToCanvasJsDataPoint(populationResult, ChromosomeFactor.ConnectedEdgeWeigthSum | ChromosomeFactor.EdgeCount)
                },
            };
        }

        public ICanvasJSDataPoint MapToDataPoint(int bestFactorSum, int iteration)
        {
            return new CanvasJSDataPoint { X = iteration, Y = bestFactorSum };
        }

        public IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> AddToDataPoints(
            IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> dataPoints,
            IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> toAdd)
        {
            foreach (var key in toAdd.Keys)
            {
                if (dataPoints.TryGetValue(key, out var value))
                {
                    dataPoints[key].AddRange(toAdd[key]);
                }
                else
                {
                    dataPoints.Add(key, toAdd[key]);
                }
            }
            return dataPoints;
        }

        public IEnumerable<ICanvasJsData> GetEvolutionChartData(IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> dataPoints, List<ICanvasJSDataPoint> bestChromosomeDataPoints )
        {
            var bestChromosome = new CanvasJsData
            {
                Type = "line",
                ShowInLegend = true,
                Name = "Best Chromosome",
                DataPoints = bestChromosomeDataPoints
            };

            var edgeCount = new CanvasJsData
            {
                Type = "line",
                ShowInLegend = true,
                Name = "Edge Count",
                AxisYIndex = 0,
                AxisYType = "secondary",
                DataPoints = dataPoints[ChromosomeFactor.EdgeCount]
            };

            var connectedEdge = new CanvasJsData
            {
                Type = "line",
                AxisYIndex = 1,
                AxisYType = "secondary",
                ShowInLegend = true,
                Name = "Connected Edge",
                DataPoints = dataPoints[ChromosomeFactor.ConnectedEdgeWeigthSum]
            };

            return new ICanvasJsData[3]
            {
               bestChromosome,
               edgeCount,
               connectedEdge
            };
        }

        private ICanvasJSDataPoint MapToCanvasJsDataPoint(IPopulationResult populationResult, ChromosomeFactor factor)
        {
            return new CanvasJSDataPoint
            {
                X = populationResult.Iteration,
                Y = populationResult.Population.Members.Sum(x => x.Factors.Where(y => y.Key == factor).Sum(z => z.Value))
            };
        }
    }
}
