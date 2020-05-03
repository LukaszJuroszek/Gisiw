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
        private int _iteration = 0;
        private GraphData _graphData;
        private GraphOptions _graphOptions;
        private ICanvasJsConfig _evolutionConfig;
        private ICanvasJsData[] _evolutionData;
        private IMatrix _matrix;
        private IPopulationResult _initializedPopulation;
        private IDictionary<ChromosomeFactor, List<ICanvasJSDataPoint>> evolutionChartData;

        [Inject] IMatrixService MatrixService { get; set; }
        [Inject] IGraphChartService GraphChartService { get; set; }
        [Inject] IGraphConsistentService GraphConsistentService { get; set; }
        [Inject] IPopulationService PopulationService { get; set; }
        [Inject] ICanvasJsChartService CanvasJsChartService { get; set; }
        [Inject] IEvolutionService EvolutionService { get; set; }

        protected override void OnInitialized()
        {
            evolutionChartData = new Dictionary<ChromosomeFactor, List<ICanvasJSDataPoint>>();
            OnGenerateGraph();
        }

        private void OnGenerateGraph()
        {
            _matrix = MatrixService.GenerateMatrix(nodeCount: _nodeCount, probability: _edgeProbability);
            _graphData = GraphChartService.GraphDataFromMatrix(_matrix);
            _graphOptions = GraphChartService.GetDefaultGraphOptions();
            _initializedPopulation = PopulationService.Initialize(_matrix, _populationSize, _maxDiffBetweenNode);
            OnGraphRender();
        }

        private void OnNext()
        {
            _iteration++;
            var iterationResult = EvolutionService.RunIteration(_initializedPopulation, _matrix, _maxDiffBetweenNode);
            var dataPoints = CanvasJsChartService.MapPopulationToDataPoints(iterationResult);
            evolutionChartData = CanvasJsChartService.AddToDataPoints(evolutionChartData, dataPoints);
            var chartData = CanvasJsChartService.GetEvolutionChartData(evolutionChartData);
            var defaultEvolutionChartOptions = CanvasJsChartService.GetBasicOptionsForEvolutionChart();

            defaultEvolutionChartOptions.Data = chartData;
            _evolutionConfig = defaultEvolutionChartOptions;
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
