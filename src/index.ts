import { Matrix } from './Matrix';
import { Chart } from 'canvasjs'
import { GraphService } from './GraphService';
import { EvolutionService } from './EvolutionService';
import { PopulationModel } from './PopulationModel';
declare var CanvasJS: any

//init
window.onload = function () {
    var dataPointsOfF1Sum = [];
    var dataPointsOfF2Sum = [];
    var iteractionCounter = 1;

    //graph settings
    var probability: number = 0.150;
    var probabilityStep: number = 0.05;
    var nodeCount: number = 30;

    //population settings
    var maxDiffBetweenEdges: number = 6;
    var maxDiffBetweenNode: number = 4;
    var probability: number = 0.4;
    var populationSize: number = 1;

    var adjensceMatrix = new Matrix(nodeCount, probability);
    new GraphService(adjensceMatrix);
    var population = new PopulationModel(adjensceMatrix, populationSize, probability, maxDiffBetweenEdges, maxDiffBetweenNode);

    console.log(population);

    dataPointsOfF1Sum.push({
        x: iteractionCounter,
        y: population.GetF1Sum(adjensceMatrix)
    });

    dataPointsOfF2Sum.push({
        x: iteractionCounter,
        y: population.GetF2Sum(adjensceMatrix)
    });

    adjensceMatrix.DepthFirstSearch();

    document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(4);
    document.getElementById("nodeCount").textContent = ("Nodes: ") + nodeCount;

    $('#generate').click(function (e) {
        e.preventDefault();
        adjensceMatrix = new Matrix(nodeCount, probability);
        new GraphService(adjensceMatrix);
        adjensceMatrix.DepthFirstSearch();
        updateChart();
    });
    function updateChart() {
        var number = Math.floor((Math.random() * 10) + 1);
        dataPointsOfF1Sum.push({
            x: iteractionCounter,
            y: number
        });
        iteractionCounter++;
        // updating legend text with  updated with y Value 
        sumChart.render();
    }
    document.getElementById("increaseProbability").addEventListener("click", function (e) {
        if (probability + probabilityStep <= 1) {
            probability += probabilityStep;
            document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(2);
        }
    }, false);
    document.getElementById("decreaseProbability").addEventListener("click", function (e) {
        if (probability - probabilityStep >= 0) {
            probability -= probabilityStep;
            document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(2);
        }
    }, false);

    var sumChart = new CanvasJS.Chart("sumChart", {
        animationEnabled: false,
        theme: "light2",

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
        data: [{
            type: "spline",
            showInLegend: true,
            name: "Sum F1(x)",
            dataPoints: dataPointsOfF1Sum
        },
        {
            type: "spline",
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
            },
            axisY: {
                interval: 1,
            },
            data: [
                {
                    type: "scatter",
                    dataPoints: [
                        { x: 2, y: 3 },
                        { x: 3, y: 2 },
                        { x: 4, y: 2 },
                        { x: 2, y: 4 },
                        { x: 4, y: 5 },
                        { x: 4, y: 6 },
                        { x: 4, y: 2 },
                        { x: 4, y: 2 },
                        { x: 1, y: 4 },
                        { x: 1, y: 2 },
                        { x: 1, y: 2 },
                        { x: 4, y: 2 },
                        { x: 5, y: 6 },
                    ]
                }
            ]
        });
    paretoChart.render();
}