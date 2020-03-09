import { Matrix } from './matrix';
import { GraphService } from './graphService';
import { EvolutionService } from './EvolutionService';
import { PopulationService } from './populationService';
import { ChromosomeModel } from './chromosomeModel';
import * as chroma from 'chroma-js';
import * as paretoService from 'pareto-frontier';

declare var CanvasJS: any
var bestChromosome: ChromosomeModel;
$(document).ready(function () {
    console.log("ready!");

    //data charts init
    var dataPointsOfF1Sum:any[] = [];
    var dataPointsOfF2Sum:any[] = [];
    var dataPointsOfF1AndF2SumOfBestChromosome:any[] = [];
    var dataPointsPareto:any[] = [];
    var iteractionCounter = 1;

    //debug settings
    var logDebug: boolean = false;

    //graph settings
    var probability: number = 0.3;
    var probabilityStep: number = 0.05;
    var nodeCount: number = 30;
    var mainContierId: string = "graphNetwork";

    //population settings
    var maxDiffBetweenNode: number = 6;
    var probabilityForChromosome: number = 0.5; //optimal value is 0.5
    var populationSize: number = 100;

    //evolutions settings
    var numberOfTournamentRounds: number = populationSize;
    var numberOfIterations: number = 5;

    //declare
    var adjensceMatrix: Matrix;
    var graphService: GraphService;
    var isConsistent: boolean;
    var populationService: PopulationService;
    var population: Array<ChromosomeModel>;
    var evoltionService: EvolutionService;

    function initialize() {
        dataPointsOfF1Sum = [];
        dataPointsOfF2Sum = [];
        dataPointsOfF1AndF2SumOfBestChromosome = [];
        dataPointsPareto = [];
        iteractionCounter = 1;

        adjensceMatrix = new Matrix();
        graphService = new GraphService(logDebug);

        bestChromosome = new ChromosomeModel();

        isConsistent = adjensceMatrix.initializeAdjensceMatrix(nodeCount, probability);

        graphService.initializeGraph(adjensceMatrix, mainContierId);

        populationService = new PopulationService(adjensceMatrix, logDebug)
        evoltionService = new EvolutionService(numberOfTournamentRounds, populationService);

        population = populationService.initializePopulation(populationSize, adjensceMatrix, probability, maxDiffBetweenNode);
    }

    //init services
    initialize();

    populationService.setStatusString("Ready");
    document.getElementById("run").addEventListener("click", function (e) {
        e.preventDefault();
        var currentBest: ChromosomeModel = new ChromosomeModel();
        populationService.setStatusString("Running epic...");
        for (var i = 0; i < numberOfIterations; i++) {
            populationService.setStatusString("Running epic... " + iteractionCounter);

            var currentBest = graphService.calculateBestChromosome(population, iteractionCounter).getCopy();
            if (currentBest.getSumOfF1AndF2() < bestChromosome.getSumOfF1AndF2())
                bestChromosome = currentBest.getCopy();

            population = evoltionService.runIteration(population, probabilityForChromosome, adjensceMatrix, maxDiffBetweenNode);

            var [sumF1, sumF2, paretoPoins] = populationService.getF1SumF2SumAndParetoPairs(population);
            if (iteractionCounter % 1000 == 0) {
                dataPointsPareto = [];
                paretoChart = generateParetoChart(dataPointsPareto);
            }
            addDataPoins(sumF1, sumF2, paretoPoins, bestChromosome.getSumOfF1AndF2());
        }
        graphService.drawBestChromosome(bestChromosome, mainContierId);
        populationService.setStatusString("best chromosome: " + bestChromosome.getStringWithSums());
        var paretoPairs = dataPointsPareto.map(point => [point.x, point.y]);
        var paretoOptimalPairs = paretoService.getParetoFrontier(paretoPairs, { optimize: 'bottomLeft' });
        dataPointsPareto.forEach(paretoPair => {
            for (let index = 0; index < paretoOptimalPairs.length; index++) {
                var element = paretoOptimalPairs[index];
                if (paretoPair.x === element[0] && paretoPair.y === element[1]) {
                    paretoPair.markerType = "triangle";
                    paretoPair.markerSize = 20;
                } else {
                    paretoPair.markerType = "circle";
                    paretoPair.markerSize = 5;
                }
            }
        });
        updateCharts();

    }, false)

    function addDataPoins(sumF1: number, sumF2: number, paretoPoins: [number, number][], sumF1AndF2OfBestChromosome: number) {
        dataPointsOfF1Sum.push({
            x: iteractionCounter,
            y: sumF1
        });

        dataPointsOfF2Sum.push({
            x: iteractionCounter,
            y: sumF2
        });

        dataPointsOfF1AndF2SumOfBestChromosome.push({
            x: iteractionCounter,
            y: sumF1AndF2OfBestChromosome
        });

        paretoPoins.forEach(pair => {
            dataPointsPareto.push({
                x: pair[0],
                y: pair[1],
                color: getColor()
            });
        });

        iteractionCounter++;
    }

    function updateCharts() {
        // updating legend text with  updated with y Value
        sumChart.render();
        paretoChart.render();
    }

    document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(4);
    document.getElementById("nodeCount").textContent = ("Nodes: ") + nodeCount;
    document.getElementById("increaseProbability").addEventListener("click", function (e) {
        e.preventDefault();
        if (probability + probabilityStep <= 1) {
            probability += probabilityStep;
            document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(2);
        }
    }, false);
    document.getElementById("decreaseProbability").addEventListener("click", function (e) {
        e.preventDefault();
        if (probability - probabilityStep >= 0) {
            probability -= probabilityStep;
            document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(2);
        }
    }, false);

    $('#generate').click(function (e) {
        e.preventDefault();

        initialize();

        document.getElementById("dfsResult").textContent = ("Consistent: ") + isConsistent;

        sumChart = generateSumChart(dataPointsOfF1Sum, dataPointsOfF2Sum, dataPointsOfF1AndF2SumOfBestChromosome);
        paretoChart = generateParetoChart(dataPointsPareto);
        populationService.setStatusString("Ready");
    });

    document.getElementById("dfsResult").textContent = ("Consistent: ") + isConsistent;

    var sumChart = generateSumChart(dataPointsOfF1Sum, dataPointsOfF2Sum, dataPointsOfF1AndF2SumOfBestChromosome);
    var paretoChart = generateParetoChart(dataPointsPareto);

    function getColor() {
        var scale = chroma.scale(['blue', 'red']);
        var color = scale(iteractionCounter / 100).hex();
        return color;
    }

});

function generateSumChart(dataPointsOfF1Sum: any[], dataPointsOfF2Sum: any[], dataPointsOfF1AndF2SumOfBestChromosome: any[]) {
    var sumChart = new CanvasJS.Chart("sumChart", {
        animationEnabled: true,
        zoomEnabled: true,
        exportEnabled: true,
        axisY: {
            includeZero: false,
        },

        axisY2: [{
            includeZero: false,
        },
        {
            includeZero: false,
        }],

        legend: {
            cursor: "pointer",
            verticalAlign: "top",
            fontSize: 22,
            fontColor: "black",
        },

        toolTip: {
            shared: false
        },

        data: [
            {
                type: "line",
                showInLegend: true,
                name: "Sum F1(x) and F2(x) of best chromosome",
                dataPoints: dataPointsOfF1AndF2SumOfBestChromosome
            },
            {
                type: "line",
                axisYType: "secondary",
                showInLegend: true,
                name: "Sum F1(x)",
                dataPoints: dataPointsOfF1Sum
            },
            {
                type: "line",
                axisYType: "secondary",
                axisYIndex: 1,
                showInLegend: true,
                name: "Sum F2(x)",
                dataPoints: dataPointsOfF2Sum
            }
        ]
    });
    sumChart.render();
    return sumChart;
}

function generateParetoChart(dataPointsPareto: any[]) {
    var paretoChart = new CanvasJS.Chart("paretoChart", {
        animationEnabled: true,
        exportEnabled: true,
        zoomEnabled: true,
        theme: "light2",
        axisX: {
            gridColor: "black",
            gridThickness: 1,
        },
        axisY: {
            gridColor: "black",
            gridThickness: 1,
            interval: 40
        },
        data: [
            {
                type: "scatter",
                dataPoints: dataPointsPareto
            }
        ]
    });
    paretoChart.render();
    return paretoChart;
}
