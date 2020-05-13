using Graph.Component;
using Graph.Component.Models.CanvasJs;
using Graph.Component.Models.CanvasJs.Data;
using Graph.Component.Models.Graph.Data;
using Graph.Component.Models.Graph.Options;
using Graph.Core.Models;
using Graph.Core.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Graph.Pages
{
    public partial class Index : ComponentBase
    {
        private int _nodeCount = 10;
        private double _edgeProbability = 0.55d;
        private int _populationSize = 100;
        private int _maxDiffBetweenNode = 6;
        private GraphData _graphData;
        private IGraphOptions _graphOptions;
        private ICanvasJsConfig _evolutionConfig;
        private IMatrix _matrix;
        private IChromosome _currentBestChromosome;
        private IPopulationResult _currentPopulation;
        private IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> _evolutionChartData;
        private List<ICanvasJSDataPoint> _evolutionBestChromosomeChartData;

        [Inject] IMatrixService MatrixService { get; set; }
        [Inject] IGraphChartService GraphChartService { get; set; }
        [Inject] IPopulationService PopulationService { get; set; }
        [Inject] ICanvasJsChartService CanvasJsChartService { get; set; }
        [Inject] IEvolutionService EvolutionService { get; set; }

        protected override void OnInitialized()
        {
            _evolutionBestChromosomeChartData = new List<ICanvasJSDataPoint>();
            _evolutionChartData = new Dictionary<ChromosomeFactor, List<ICanvasJSDataPoint>>();

            OnInitGraph();
            InitEvolutionChart();
            OnGraphRender();
            OnEvolutionChartRender();
        }

        private void OnInitGraph()
        {
            _matrix = MatrixService.GenerateMatrix(nodeCount: _nodeCount, probability: _edgeProbability);
            _graphData = GraphChartService.GraphDataFromMatrix(_matrix);
            _graphOptions = GraphChartService.GetDefaultGraphOptions();
            _currentPopulation = PopulationService.Initialize(_matrix, _populationSize, _maxDiffBetweenNode);
            _currentBestChromosome = _currentPopulation.BestChromosome;
        }

        private void OnNext()
        {
            _evolutionConfig = CanvasJsChartService.GetBasicOptionsForEvolutionChart();

            for (var i = 0; i < 10; i++)
            {
                _currentPopulation = EvolutionService.RunIteration(_currentPopulation, _matrix, _maxDiffBetweenNode);
                _currentBestChromosome = _currentBestChromosome.FactorsSum < _currentPopulation.BestChromosome.FactorsSum ? _currentBestChromosome.DeepCopy() : _currentPopulation.BestChromosome.DeepCopy();

                _evolutionBestChromosomeChartData.Add(CanvasJsChartService.MapToDataPoint(_currentBestChromosome.FactorsSum, _currentPopulation.Iteration));

                var dataPoints = CanvasJsChartService.MapPopulationToDataPoints(_currentPopulation);
                _evolutionChartData = CanvasJsChartService.AddToDataPoints(_evolutionChartData, dataPoints);
            }
            _evolutionConfig.Data = CanvasJsChartService.GetEvolutionChartData(_evolutionChartData, _evolutionBestChromosomeChartData);

            _graphData = GraphChartService.GraphDataFromChromosome(_currentBestChromosome, _matrix);
            _graphOptions = GraphChartService.GetBestChromosomeGraphOptions();
            OnGraphRender();
            OnEvolutionChartRender();
        }

        private void InitEvolutionChart()
        {
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
            _evolutionConfig = CanvasJsChartService.GetBasicOptionsForEvolutionChart();
            _evolutionConfig.Data = CanvasJsChartService.GetEvolutionChartData(sampleDictionary, sampleBest);
        }
    }
}
