import { Matrix } from './Matrix';
import {Chart } from 'canvasjs'
import { GraphService } from './GraphService';
import { EvolutionService } from './EvolutionService';
declare var CanvasJS:any

//init
window.onload = function () {
    var dataPoints1 = [];
    var cunter = 1;

    var probability = 0.150;
    var nodeCount = 30;
    var range = 0.05;
    var adjensceMatrix = new Matrix(nodeCount, probability);
    new GraphService(adjensceMatrix);
    var evModel = new EvolutionService(adjensceMatrix);
    for (let index = 0; index < 10; index++) {
        var p = evModel.CreateEvolutionModelFromMatrix();
        console.log(p);
    }
    console.log(CanvasJS);
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
    document.getElementById("increaseProbability").addEventListener("click", function (e) {
        if (probability + range <= 1) {
            probability += range;
            document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(2);
        }
    }, false);
    document.getElementById("decreaseProbability").addEventListener("click", function (e) {
        if (probability - range >= 0) {
            probability -= range;
            document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(2);
        }
    }, false);

    function updateChart() {
        var number = Math.floor((Math.random() * 10) + 1);
        dataPoints1.push({
            x: cunter,
            y: number
        });
        cunter++;
        // updating legend text with  updated with y Value 
        sumChart.render();
    }

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
            dataPoints: dataPoints1
        },
        {
            type: "spline",
            showInLegend: true,
            name: "Sum F2(x)",
            dataPoints: [
                { y: 5 },
                { y: 1 },
                { y: 2 },
                { y: 3 },
                { y: 1 }
            ]
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