import { Matrix } from './Matrix';
import { GraphService } from './GraphService';
import { EvolutionService } from './EvolutionService';
import { PopulationModel } from './PopulationModel';
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
    var nodeCount: number = 6;

    //population settings
    var maxDiffBetweenEdges: number = 6;
    var maxDiffBetweenNode: number = 4;
    var probability: number = 0.4;
    var populationSize: number = 100;

    //evolutions settings
    var numberOfTournamentRounds: number = populationSize / 2;
    var numberOfIterations: number = 100;

    //init of matrix
    var adjensceMatrix = new Matrix(nodeCount, probability);
    new GraphService(adjensceMatrix);

    var isConsistent = adjensceMatrix.DepthFirstSearch();

    //init population
    var population = new PopulationModel(adjensceMatrix, populationSize, probability, maxDiffBetweenEdges, maxDiffBetweenNode, logDebug);
    //init evolution
    var ev = new EvolutionService(population, adjensceMatrix, numberOfTournamentRounds, logDebug);
    // ev.iterateBy();

    console.log(population);

    document.getElementById("run").addEventListener("click", function (e) {
        e.preventDefault();
        for (let i = 0; i < numberOfIterations; i++) {
            var populationAfterIteration = ev.runIteration();
            var sumF1 = populationAfterIteration.getF1Sum();
            var sumF2 = populationAfterIteration.getF2Sum();
            var paretoPoins = populationAfterIteration.getParetoPairs();
            updateDataPointsOfF1AndF2Sum(sumF1, sumF2);
            updateDataParetoChart(paretoPoins);
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
                y: pair[1]
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
        adjensceMatrix.DepthFirstSearch();
    });

    document.getElementById("dfsResult").textContent = ("Consistent: ") + isConsistent;

    var sumChart = new CanvasJS.Chart("sumChart", {
        animationEnabled: false,
        theme: "light2",
        zoomEnabled: true,
        title: {
            text: "Simple Line Chart"
        },
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
    var paretoChart = new CanvasJS.Chart("paretoChart",
        {
            title: {
                text: "Pareto front"
            },
            axisX: {
                gridThickness: 1,
                interval: 1,
                title: "F1",
            },
            axisY: {
                interval: 1,
                title: "F2",
            },
            data: [
                {
                    type: "scatter",
                    dataPoints: dataPointsPareto
                }
            ]
        });
    paretoChart.render();

}