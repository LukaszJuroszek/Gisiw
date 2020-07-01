using Graph.Component.Models.CanvasJs.Data;
using Graph.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Graph.Core.Services
{
    public interface IParetoService
    {
        IEnumerable<ICanvasJSDataPoint> GetParetoFrontier(Dictionary<int, IPopulationResult> populationHistory);
    }

    internal class ColoredData
    {
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; }
        public int Iteration { get; set; }
        public bool IsInParetoFront { get; set; }
    }

    public class ParetoService : IParetoService
    {
        private readonly ICanvasJsChartService _canvasJsChartService;

        public ParetoService(ICanvasJsChartService canvasJsChartService)
        {
            _canvasJsChartService = canvasJsChartService;
        }

        public IEnumerable<ICanvasJSDataPoint> GetParetoFrontier(Dictionary<int, IPopulationResult> populationHistory)
        {
            var coloredData = new List<ColoredData>();

            foreach (var iteration in populationHistory.Keys)
            {
                var chromosomes = populationHistory[iteration].Population.Members;
                var color = GetColor(iteration);
                coloredData.AddRange(chromosomes.Select(x => new ColoredData
                {
                    Id = x.Id,
                    Y = x.Factors[ChromosomeFactor.EdgeCount],
                    X = x.Factors[ChromosomeFactor.ConnectedEdgeWeigthSum],
                    Color = color,
                    Iteration = iteration
                }));
            }

            coloredData = coloredData.GroupBy(x => new { x.Y, x.X, }, x => x)
                                     .Select(x => x.OrderByDescending(y => y.Iteration).FirstOrDefault()).ToList();

            foreach (var data in coloredData)
            {
                var s = coloredData.Any(p => data.X > p.X);
                var z = coloredData.Any(p => data.Y > p.Y);
                if (s && z)
                {
                    data.IsInParetoFront = true;
                }
                else
                {
                    data.IsInParetoFront = false;
                }
            }

            return coloredData.Select(x => _canvasJsChartService.MapToDataPoint(x.X, x.Y, x.Color, x.IsInParetoFront));
        }

        private string GetColor(int iteration)
        {
            var baseColor = iteration < 100 ? 1 : iteration / 100;

            var color = Color.FromArgb(255, baseColor, 255 - iteration / baseColor, 0);

            return $"rgb({color.R},{color.G},{color.B})";
        }
    }
}
