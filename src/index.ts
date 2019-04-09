import { Matrix } from './matrix';
import { GraphService } from './graphService';
import { EvolutionService } from './EvolutionService';
import { PopulationService } from './populationService';
import { ChromosomeModel } from './chromosomeModel';
declare var CanvasJS: any
var bestChromosome: ChromosomeModel;

//init
window.onload = function () {
    //data charts init
    var dataPointsOfF1Sum = [];
    var dataPointsOfF2Sum = [];
    var dataPointsPareto = [];
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
    var numberOfTournamentRounds: number = populationSize / 2;
    var numberOfIterations: number = 10;

    //declare
    var adjensceMatrix: Matrix;
    var graphService: GraphService;
    var isConsistent: boolean;
    var populationService: PopulationService;
    var population: Array<ChromosomeModel>;
    var evoltionService: EvolutionService;

    function initialize() {
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
        console.log("----------------------------------------------------------------");
        console.log("currentBest.getSumOfF1AndF2(): " + currentBest.getSumOfF1AndF2());
        console.log("bestChromosome.getSumOfF1AndF2() " + bestChromosome.getSumOfF1AndF2());
        populationService.setStatusString("Running epic...");
        for (var i = 0; i < numberOfIterations; i++) {
            populationService.setStatusString("Running epic... " + iteractionCounter);

            var tempBest = graphService.createGraphForBestChromosome(mainContierId, population, iteractionCounter);
            if (tempBest.getSumOfF1AndF2() < currentBest.getSumOfF1AndF2()) {
                currentBest = tempBest;
            }
            console.log("currentBest.getSumOfF1AndF2(): " + currentBest.getSumOfF1AndF2());

            population = evoltionService.runIteration(population, probabilityForChromosome, adjensceMatrix, maxDiffBetweenNode);

            var [sumF1, sumF2, paretoPoins] = populationService.getF1SumF2SumAndParetoPairs(population);
            addDataPoins(sumF1, sumF2, paretoPoins);
        }
        updateCharts();
        console.log("++++++++++++++++++++++++++++++++++++++++++++++++");
        console.log("currentBest.getSumOfF1AndF2(): " + currentBest.getSumOfF1AndF2());
        console.log("bestChromosome.getSumOfF1AndF2() " + bestChromosome.getSumOfF1AndF2());

        if (currentBest.getSumOfF1AndF2() < bestChromosome.getSumOfF1AndF2()) {
            bestChromosome = currentBest;
        }

        populationService.setStatusString("best chromosome: " + bestChromosome.getStringWithSums());
        console.log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        console.log("currentBest.getSumOfF1AndF2(): " + currentBest.getSumOfF1AndF2());
        console.log("bestChromosome.getSumOfF1AndF2() " + bestChromosome.getSumOfF1AndF2());

    }, false);

    function addDataPoins(sumF1: number, sumF2: number, paretoPoins: [number, number][]) {
        dataPointsOfF1Sum.push({
            x: iteractionCounter,
            y: sumF1
        });

        dataPointsOfF2Sum.push({
            x: iteractionCounter,
            y: sumF2
        });

        paretoPoins.forEach(pair => {
            dataPointsPareto.push({
                x: pair[0],
                y: pair[1],
                color: getRandomColor()
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

        //clear charts data 
        dataPointsOfF1Sum = [];
        dataPointsOfF2Sum = [];
        dataPointsPareto = [];
        iteractionCounter = 1;
        bestChromosome = new ChromosomeModel();
        sumChart = generateSumChart(dataPointsOfF1Sum, dataPointsOfF2Sum);
        paretoChart = generateParetoChart(dataPointsPareto);
        populationService.setStatusString("Ready");
    });

    document.getElementById("dfsResult").textContent = ("Consistent: ") + isConsistent;

    var sumChart = generateSumChart(dataPointsOfF1Sum, dataPointsOfF2Sum);
    var paretoChart = generateParetoChart(dataPointsPareto);

    function perc2color(perc) {
        var r, g, b = 0;
        if (perc < 50) {
            r = 255;
            g = Math.round(5.1 * perc);
        }
        else {
            g = 255;
            r = Math.round(510 - 5.10 * perc);
        }
        var h = r * 0x10000 + g * 0x100 + b * 0x1;
        return '#' + ('000000' + h.toString(16)).slice(-6);
    }

    function getRandomColor() {
        var letters = '0123456789ABCDEF';
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }
}

function generateSumChart(dataPointsOfF1Sum: any[], dataPointsOfF2Sum: any[]) {
    var sumChart = new CanvasJS.Chart("sumChart", {
        animationEnabled: false,
        theme: "light2",
        zoomEnabled: true,
        axisY: {
            includeZero: false
        },
        legend: {
            cursor: "pointer",
            verticalAlign: "top",
            fontSize: 22,
            fontColor: "black",
        },
        toolTip: {
            shared: true
        },
        data: [{
            type: "line",
            showInLegend: true,
            name: "Sum F1(x)",
            dataPoints: dataPointsOfF1Sum
        },
        {
            axisYType: "secondary",
            axisYIndex: 1,
            type: "line",
            showInLegend: true,
            name: "Sum F2(x)",
            dataPoints: dataPointsOfF2Sum
        }]
    });
    sumChart.render();
    return sumChart;
}

function generateParetoChart(dataPointsPareto: any[]) {
    var paretoChart = new CanvasJS.Chart("paretoChart", {
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
