using Graph.Component.Models.CanvasJs;
using Graph.Component.Models.CanvasJs.Data;
using Graph.Component.Models.CanvasJs.Options;
using Graph.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface ICanvasJsChartService
    {
        IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> AddToDataPoints(IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> dataPoints,
            IDictionary<ChromosomeFactor, IList<ICanvasJSDataPoint>> toAdd);
        ICanvasJsConfig GetBasicOptionsForEvolutionChart();
        IEnumerable<ICanvasJsData> GetEvolutionChartData(IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> dataPoints);
        IDictionary<ChromosomeFactor, IList<ICanvasJSDataPoint>> MapPopulationToDataPoints(IPopulationResult populationResult);
    }

    public class CanvasJsChartService : ICanvasJsChartService
    {
        public ICanvasJsConfig GetBasicOptionsForEvolutionChart()
        {
            return new CanvasJsConfig
            {
                AnimationEnabled = true,
                ZoomEnabled = true,
                ExportEnabled = true,
                AxisY = new AxisOptions
                {
                    IncludeZero = true
                },
                AxisY2 = new IAxisOptions[]
                {
                    new AxisOptions
                    {
                        IncludeZero = true
                    },
                     new AxisOptions
                    {
                        IncludeZero = true
                    },
                },
                Legend = new LegendOptions
                {
                    Cursor = "pointer",
                    VerticalAlign = "top",
                    FontSize = 22,
                    FontColor = "black",
                },
                ToolTip = new ToolTipOptions
                {
                    Shared = false
                }
            };
        }

        public IDictionary<ChromosomeFactor, IList<ICanvasJSDataPoint>> MapPopulationToDataPoints(IPopulationResult populationResult)
        {
            return new Dictionary<ChromosomeFactor, IList<ICanvasJSDataPoint>>()
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

        public IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> AddToDataPoints(
            IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> dataPoints,
            IDictionary<ChromosomeFactor, IList<ICanvasJSDataPoint>> toAdd)
        {
            foreach (var key in dataPoints.Keys)
            {
                if (dataPoints[key] != null)
                {
                    dataPoints[key].AddRange(toAdd[key]);
                }
            }
            return dataPoints;
        }

        public IEnumerable<ICanvasJsData> GetEvolutionChartData(IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> dataPoints)
        {
            return new ICanvasJsData[3]
            {
                new CanvasJsData
                {
                    Type = "line",
                    ShowLegend = true,
                    Name = "Edge Count and Connected Edge",
                    DataPoints = dataPoints[ChromosomeFactor.EdgeCount | ChromosomeFactor.ConnectedEdgeWeigthSum]
                },
                new CanvasJsData
                {
                    Type = "line",
                    AxisYType = "secondary",
                    ShowLegend = true,
                    Name = "Edge Count Sum",
                    DataPoints = dataPoints[ChromosomeFactor.EdgeCount]
                },
                new CanvasJsData
                {
                    Type = "line",
                    AxisYType = "secondary",
                    AxisYIndex = 1,
                    ShowLegend = true,
                    Name = "Connected Edge Sum",
                    DataPoints = dataPoints[ChromosomeFactor.ConnectedEdgeWeigthSum]
                },
            };
        }

        private ICanvasJSDataPoint MapToCanvasJsDataPoint(IPopulationResult populationResult, ChromosomeFactor factor)
        {
            return new CanvasJSDataPoint
            {
                X = populationResult.Iteration,
                Y = populationResult.Population.Members.Sum(x => x.Factors.Where(y => y.Key.HasFlag(factor)).Sum(z => z.Value))
            };
        }
    }
}
