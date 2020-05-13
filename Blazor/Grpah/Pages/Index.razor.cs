using Graph.Component.Models.CanvasJs;
using Graph.Component.Models.CanvasJs.Data;
using Graph.Component.Models.Graph.Data;
using Graph.Component.Models.Graph.Options;
using Graph.Core.Models;
using Graph.Core.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Graph.Pages
{
    public partial class Index : ComponentBase
    {
        private int _nodeCount = 30;
        private double _edgeProbability = 0.2d;
        private int _populationSize = 50;
        private int _maxDiffBetweenNode = 6;

        private IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> _evolutionChartData;
        private List<ICanvasJSDataPoint> _evolutionBestChromosomeChartData;

        private IMatrix _matrix;
        private IPopulationResult _startPopulation;

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
            _evolutionBestChromosomeChartData = new List<ICanvasJSDataPoint>();
            _evolutionChartData = new Dictionary<ChromosomeFactor, List<ICanvasJSDataPoint>>();
            _evolutionConfig = CanvasJsChartService.GetBasicOptionsForEvolutionChart();
            _matrix = MatrixService.GenerateMatrix(nodeCount: _nodeCount, probability: _edgeProbability);
            _startPopulation = PopulationService.Initialize(_matrix, populationSize: _populationSize, maxDiffBetweenNode: _maxDiffBetweenNode);

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

            await Task.Run(() => { _graphChart.RenderAsync(_graphData, _graphOptions); });
            await Task.Run(() => { _evolutionChart.RenderAsync(_evolutionConfig); });
        }

        private void OnIteration()
        {
            //_evolutionConfig = CanvasJsChartService.GetBasicOptionsForEvolutionChart();

            //for (var i = 0; i < 10; i++)
            //{
            //    _currentPopulation = EvolutionService.RunIteration(_currentPopulation, _matrix, _maxDiffBetweenNode);
            //    _currentBestChromosome = _currentBestChromosome.FactorsSum < _currentPopulation.BestChromosome.FactorsSum ? _currentBestChromosome.DeepCopy() : _currentPopulation.BestChromosome.DeepCopy();

            //    _evolutionBestChromosomeChartData.Add(CanvasJsChartService.MapToDataPoint(_currentBestChromosome.FactorsSum, _currentPopulation.Iteration));

            //    var dataPoints = CanvasJsChartService.MapPopulationToDataPoints(_currentPopulation);
            //    _evolutionChartData = CanvasJsChartService.AddToDataPoints(_evolutionChartData, dataPoints);
            //}
            //IndexService.RunIteration.(nodeCount: _nodeCount,
            //                                        edgeProbability: _edgeProbability,
            //                                        populationSize: _populationSize,
            //                                        maxDiffBetweenNode: _maxDiffBetweenNode);

            //_evolutionConfig.Data = CanvasJsChartService.GetEvolutionChartData(_evolutionChartData, _evolutionBestChromosomeChartData);

            //_graphData = GraphChartService.GraphDataFromChromosome(_currentBestChromosome, _matrix);
            //_graphOptions = GraphChartService.GetBestChromosomeGraphOptions();
            //OnGraphRender();
            //OnEvolutionChartRender();
        }



    }
}
