using Graph.Component;
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
        private GraphChart _graphChart;
        private CanvasJsChart _evolutionChart;
        private int _nodeCount = 10;
        private double _edgeProbability = 0.55d;
        private int _populationSize = 100;
        private int _maxDiffBetweenNode = 6;
        private int _iteration = 1;
        private GraphData _graphData;
        private GraphOptions _graphOptions;
        private ICanvasJsConfig _evolutionConfig;
        private IMatrix _matrix;
        private IPopulationResult _initializedPopulation;
        private IDictionary<int, IPopulationResult> _populationHistory;
        private IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> evolutionChartData;

        [Inject] IMatrixService MatrixService { get; set; }
        [Inject] IGraphChartService GraphChartService { get; set; }
        [Inject] IPopulationService PopulationService { get; set; }
        [Inject] ICanvasJsChartService CanvasJsChartService { get; set; }
        [Inject] IEvolutionService EvolutionService { get; set; }

        protected override void OnInitialized()
        {
            OnGenerateGraph();
        }

        private void OnGenerateGraph()
        {
            _matrix = MatrixService.GenerateMatrix(nodeCount: _nodeCount, probability: _edgeProbability);
            _graphData = GraphChartService.GraphDataFromMatrix(_matrix);
            _graphOptions = GraphChartService.GetDefaultGraphOptions();
            _initializedPopulation = PopulationService.Initialize(_matrix, _populationSize, _maxDiffBetweenNode);
            _populationHistory = new Dictionary<int, IPopulationResult> { [_iteration] = _initializedPopulation };
            OnGraphRender();
        }

        private void OnNext()
        {
            _evolutionConfig = CanvasJsChartService.GetBasicOptionsForEvolutionChart();
            if (evolutionChartData == null)
            {
                evolutionChartData = new Dictionary<ChromosomeFactor, List<ICanvasJSDataPoint>>();
            }
            for (int i = 0; i < 1; i++)
            {

                var iterationResult = EvolutionService.RunIteration(_populationHistory[_iteration], _matrix, _maxDiffBetweenNode);
                _iteration++;
                _populationHistory.Add(_iteration, iterationResult);
                var dataPoints = CanvasJsChartService.MapPopulationToDataPoints(_populationHistory[_iteration]);
                evolutionChartData = CanvasJsChartService.AddToDataPoints(evolutionChartData, dataPoints);
            }

            _evolutionConfig.Data = CanvasJsChartService.GetEvolutionChartData(evolutionChartData);
            OnEvolutionChartRender();
        }

        private async void OnRender()
        {
            await Task.Run(() =>
            {
                OnGraphRender();
                OnEvolutionChartRender();
            });

        }

        private async void OnGraphRender()
        {
            await Task.Run(() => _graphChart.Render());
        }

        private async void OnEvolutionChartRender()
        {
            await Task.Run(() => _evolutionChart.Render());
        }
    }
}
