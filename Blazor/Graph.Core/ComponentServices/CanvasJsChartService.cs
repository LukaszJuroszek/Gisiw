using Graph.Component.Models.CanvasJs;
using Graph.Component.Models.CanvasJs.Data;
using Graph.Component.Models.CanvasJs.Options;
using Graph.Core.Models;
using System;
using System.Linq;

namespace Graph.Core.Services
{
    public interface ICanvasJsChartService
    {
        ICanvasJSDataPoint[] GetDataPoint(IPopulation population);
        ICanvasJsConfig GetBasicOptionsForEvolutionChart();
        ICanvasJsConfig GetDefaultCanvasJSConfigs(ICanvasJsData[] data, ICanvasJsConfig config);
        ICanvasJsData[] GetEvolutionChartData(IPopulation population, int iteration = 0);
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

        public ICanvasJSDataPoint[] GetDataPoint(IPopulation population)
        {
            throw new NotImplementedException();
        }

        public ICanvasJsConfig GetDefaultCanvasJSConfigs(ICanvasJsData[] data, ICanvasJsConfig config)
        {
            if (config == null)
            {
                throw new Exception("config is null");
            }
            config.Data = data;
            return config;
        }

        public ICanvasJsData[] GetEvolutionChartData(IPopulation population, int iteration = 0)
        {
            return new ICanvasJsData[3]
            {
                new CanvasJsData
                {
                    Type = "line",
                    ShowLegend = true,
                    Name = "Edge Count and Connected Edge",
                    DataPoints = GetEdgeCountData(population, ChromosomeFactor.EdgeCount | ChromosomeFactor.ConnectedEdgeWeigthSum, iteration)
                },
                new CanvasJsData
                {
                    Type = "line",
                    AxisYType = "secondary",
                    ShowLegend = true,
                    Name = "Edge Count Sum",
                    DataPoints = GetEdgeCountData(population, ChromosomeFactor.EdgeCount, iteration)
                },
                new CanvasJsData
                {
                    Type = "line",
                    AxisYType = "secondary",
                    AxisYIndex = 1,
                    ShowLegend = true,
                    Name = "Edge Count and Connected Edge",
                    DataPoints = GetEdgeCountData(population, ChromosomeFactor.ConnectedEdgeWeigthSum, iteration)
                },
            };
        }

        public ICanvasJSDataPoint[] GetEdgeCountData(IPopulation population, ChromosomeFactor factor, int iteration)
        {
            return population.Members.Select(x => new CanvasJSDataPoint
            {
                X = iteration,
                Y = x.Factors.Where(y => y.Key.HasFlag(factor)).Sum(x => x.Value)
            }).ToArray();
        }
    }
}
