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

    public class ColoredData
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

            var groupedColoredData = coloredData.GroupBy(x => new { x.Y, x.X, }, x => x)
                                             .Select(x => new
                                             {
                                                 ColoredData = x.OrderByDescending(y => y.Iteration).FirstOrDefault(),
                                                 Count = x.Count() < 10 ? 10 : x.Count()
                                             }).ToList();

            foreach (var data in groupedColoredData)
            {
                var s = coloredData.Any(p => data.ColoredData.X > p.X);
                var z = coloredData.Any(p => data.ColoredData.Y > p.Y);
                if (s && z)
                {
                    data.ColoredData.IsInParetoFront = true;
                }
                else
                {
                    data.ColoredData.IsInParetoFront = false;
                }
                Console.WriteLine(data.Count);
            }
            return groupedColoredData.Select(x => _canvasJsChartService.MapToDataPoint(x.ColoredData, x.Count));

        }

        private string GetColor(int iteration)
        {
            var baseColor = iteration < 100 ? 1 : iteration / 100;

            var color = Color.FromArgb(255, baseColor, 255 - iteration / baseColor, 125);

            return $"rgb({color.R},{color.G},{color.B})";
        }
    }
}
