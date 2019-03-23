import { Matrix } from './Matrix';
import { GraphService } from './GraphService';
import { EvolutionService } from './EvolutionService';
import { PopulationModel } from './PopulationModel';
import { PopulationService } from './PopulationService';
import { ChromosomeElement, ChromosomeModel } from './ChromosomeModel';
declare var CanvasJS: any

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
    var probability: number = 0.150;
    var probabilityStep: number = 0.05;
    var nodeCount: number = 30;

    //population settings
    var maxDiffBetweenEdges: number = 6;
    var maxDiffBetweenNode: number = 4;
    var probabilityForChromosome: number = 0.3;
    var populationSize: number = 100;

    //evolutions settings
    var numberOfTournamentRounds: number = populationSize / 2;
    var numberOfIterations: number = 100;

    //init of matrix
    var adjensceMatrix = new Matrix(nodeCount, probability);
    new GraphService(adjensceMatrix);
    var isConsistent = adjensceMatrix.DepthFirstSearch();


    //init population
    var popService = new PopulationService(adjensceMatrix, probabilityForChromosome, maxDiffBetweenEdges, maxDiffBetweenNode, logDebug)
    var population = popService.generatePopulationOrAddMissingIfPopulationSize(new PopulationModel(new Set<ChromosomeModel>()), populationSize);
    //init evolution
    var ev = new EvolutionService(numberOfTournamentRounds, popService, logDebug);

    document.getElementById("run").addEventListener("click", function (e) {
        e.preventDefault();
        var x = 0;
        for (let i = 0; i < numberOfIterations; i++) {
            population = ev.runIteration(population);
            var [sumF1, sumF2, paretoPoins] = population.getF1SumF2SumAndParetoPairs();
            updateDataPointsOfF1AndF2Sum(sumF1, sumF2);
            if (x === i) {
                x = x === 0 ? 1 : x;
                updateDataParetoChart(paretoPoins);
                x *= 10;
            }
        }
    }, false);

    function updateDataPointsOfF1AndF2Sum(sumF1: number, sumF2: number) {
        dataPointsOfF1Sum.push({
            x: iteractionCounter,
            y: sumF1
        });

        dataPointsOfF2Sum.push({
            x: iteractionCounter,
            y: sumF2
        });
        iteractionCounter++;
        // updating legend text with  updated with y Value 
        sumChart.render();
    }

    function updateDataParetoChart(paretoPoins: [number, number][]) {
        paretoPoins.forEach(pair => {
            dataPointsPareto.push({
                x: pair[0],
                y: pair[1],
                color: perc2color(iteractionCounter /10)
            });
        });
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
        adjensceMatrix = new Matrix(nodeCount, probability);
        new GraphService(adjensceMatrix);
        isConsistent = adjensceMatrix.DepthFirstSearch();
        //init population
        population = popService.generatePopulationOrAddMissingIfPopulationSize(new PopulationModel(new Set<ChromosomeModel>()), populationSize);
        document.getElementById("dfsResult").textContent = ("Consistent: ") + isConsistent;

        //clear charts data 
         dataPointsOfF1Sum = [];
         dataPointsOfF2Sum = [];
         dataPointsPareto = [];
         iteractionCounter = 1;
         sumChart = generateSumChart(dataPointsOfF1Sum, dataPointsOfF2Sum);
         paretoChart = generateParetoChart(dataPointsPareto);
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
