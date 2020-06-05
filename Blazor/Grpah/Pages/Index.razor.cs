using Graph.Component.Models.CanvasJs.Data;
using Graph.Core.Models;
using Graph.Core.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graph.Pages
{
    public partial class Index : ComponentBase
    {
        private int _nodeCount = 30;
        private double _edgeProbability = 0.2d;
        private int _populationSize = 10;
        private int _maxDiffBetweenNode = 3;

        private IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> _evolutionChartData;
        private List<ICanvasJSDataPoint> _bestChromosomeChartData;

        private IMatrix _matrix;
        private List<IPopulationResult> _populationHistory;

        private List<ICanvasJSDataPoint> _paretoChartData;

        [Inject] IEvolutionService EvolutionService { get; set; }
        [Inject] IMatrixService MatrixService { get; set; }
        [Inject] IPopulationService PopulationService { get; set; }
        [Inject] ICanvasJsChartService CanvasJsChartService { get; set; }
        [Inject] IGraphChartService GraphChartService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await StartAsync();
        }

        public async Task StartAsync()
        {
            _paretoChartData = new List<ICanvasJSDataPoint>();
            _bestChromosomeChartData = new List<ICanvasJSDataPoint>();
            _evolutionChartData = new Dictionary<ChromosomeFactor, List<ICanvasJSDataPoint>>();

            _evolutionConfig = CanvasJsChartService.GetBasicOptionsForEvolutionChart();
            _paretoConfig = CanvasJsChartService.GetBasicOptionsForParetoChart();
            _matrix = MatrixService.GenerateMatrix(nodeCount: _nodeCount, probability: _edgeProbability);

            var startPopulation = PopulationService.Initialize(_matrix, populationSize: _populationSize,
                maxDiffBetweenNode: _maxDiffBetweenNode);
            _populationHistory = new List<IPopulationResult> { startPopulation };

            _graphData = GraphChartService.GraphDataFromMatrix(_matrix);
            _graphOptions = GraphChartService.GetDefaultGraphOptions();

            var sampleDictionary = new Dictionary<ChromosomeFactor, List<ICanvasJSDataPoint>>
            {
                [ChromosomeFactor.ConnectedEdgeWeigthSum] = new List<ICanvasJSDataPoint>
                {
                    new CanvasJSDataPoint{X = 0, Y = 0},
                },
                [ChromosomeFactor.EdgeCount] = new List<ICanvasJSDataPoint>
                {
                    new CanvasJSDataPoint{X = 0, Y = 0},
                },
                [ChromosomeFactor.ConnectedEdgeWeigthSum | ChromosomeFactor.EdgeCount] = new List<ICanvasJSDataPoint>
                {
                    new CanvasJSDataPoint{X = 0, Y = 0},
                }
            };
            var sampleBest = new List<ICanvasJSDataPoint> { new CanvasJSDataPoint { X = 0, Y = 0 } };

            _evolutionConfig.Data = CanvasJsChartService.GetEvolutionChartData(sampleDictionary, sampleBest);

            _paretoConfig.Data = CanvasJsChartService.GetParetoChartData(new List<ICanvasJSDataPoint>
            {
                new CanvasJSDataPoint{X = 0, Y = 0},
            });

            await Task.Run(() => { _graphChart.RenderAsync(_graphData, _graphOptions); });
            await Task.Run(() => { _evolutionChart.RenderAsync(_evolutionConfig); });
            await Task.Run(() => { _paretoChart.RenderAsync(_paretoConfig); });
        }

        public async Task OnIterationAsync(int iterations)
        {
            var lastPopulation = _populationHistory.OrderByDescending(x => x.Iteration).FirstOrDefault();

            var newPopulationHistory = EvolutionService.RunIterations(iterations: iterations,
                                                                      maxDiffBetweenNode: _maxDiffBetweenNode,
                                                                      population: lastPopulation,
                                                                      matrix: _matrix);

            var bestChromosomeFromHistory = _populationHistory.Select(x => x.BestChromosome)
                .OrderBy(x => x.FactorsSum)
                .FirstOrDefault()
                .DeepCopy();

            _populationHistory.AddRange(newPopulationHistory);

            foreach (var population in _populationHistory.Where(x => x.Iteration > lastPopulation.Iteration))
            {
                bestChromosomeFromHistory = bestChromosomeFromHistory.FactorsSum < population.BestChromosome.FactorsSum ?
                     bestChromosomeFromHistory.DeepCopy() : population.BestChromosome.DeepCopy();

                _bestChromosomeChartData.Add(CanvasJsChartService.MapToDataPoint(bestChromosomeFromHistory.FactorsSum, population.Iteration));

                var dataPoints = CanvasJsChartService.MapPopulationToDataPoints(population);
                _evolutionChartData = CanvasJsChartService.AddToDataPoints(_evolutionChartData, dataPoints);

                _paretoChartData.AddRange(population.Population.Members.Select(x => CanvasJsChartService.MapToDataPoint(x)));

            }

            _paretoConfig.Data = CanvasJsChartService.GetParetoChartData(_paretoChartData);

            _evolutionConfig.Data = CanvasJsChartService.GetEvolutionChartData(_evolutionChartData, _bestChromosomeChartData);

            _graphData = GraphChartService.GraphDataFromChromosome(bestChromosomeFromHistory, _matrix);
            _graphOptions = GraphChartService.GetBestChromosomeGraphOptions();

            await Task.Run(() => { _graphChart.RenderAsync(_graphData, _graphOptions); });
            await Task.Run(() => { _evolutionChart.RenderAsync(_evolutionConfig); });
            await Task.Run(() => { _paretoChart.RenderAsync(_paretoConfig); });
        }
    }
}
