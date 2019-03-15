import { Matrix } from './Matrix';
import * as CanvasJS from 'canvasjs';
import { GraphService } from './GraphService';
import { EvolutionService } from './EvolutionService';

//init
$(document).ready(function () {
    var probability = 0.150;
    var nodeCount = 30;
    var range = 0.05;
    var adjensceMatrix = new Matrix(nodeCount, probability);
    new GraphService(adjensceMatrix);
    var p = new EvolutionService(adjensceMatrix).CreateEvolutionModelFromMatrix();
    console.log(p);
    adjensceMatrix.DepthFirstSearch();
    document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(4);
    document.getElementById("nodeCount").textContent = ("Nodes: ") + nodeCount;
    $('#generate').click(function (e) {
        e.preventDefault();
        adjensceMatrix = new Matrix(nodeCount, probability);
        new GraphService(adjensceMatrix);
        adjensceMatrix.DepthFirstSearch();
    });
    document.getElementById("increaseProbability").addEventListener("click", function (e) {
        if (probability + range <= 1) {
            probability += range;
            document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(4);
        }
    }, false);
    document.getElementById("decreaseProbability").addEventListener("click", function (e) {
        if (probability - range >= 0) {
            probability -= range;
            document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(2);
        }
    }, false);
});
window.onload = function () {

    var sumChart = new CanvasJS.Chart("sumChart", {
        animationEnabled: false,
        theme: "light2",
        title: {
            text: "Simple Line Chart"
        },
        axisY: {
            includeZero: false
        },
        data: [{
            type: "line",
            dataPoints: [
                { y: 450 },
                { y: 414 },
                { y: 520 },
                { y: 480 },
                { y: 510 }
            ]
        }]
    });
    sumChart.render();
    var sumChart2 = new CanvasJS.Chart("sumChart2", {
        animationEnabled: false,
        theme: "light2",
        title: {
            text: "Simple Line Chart2"
        },
        axisY: {
            includeZero: false
        },
        data: [{
            type: "line",
            dataPoints: [
                { y: 450 },
                { y: 414 },
                { y: 520 },
                { y: 480 },
                { y: 510 }
            ]
        }]
    });
    sumChart2.render();
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