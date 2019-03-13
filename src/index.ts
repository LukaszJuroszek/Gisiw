import { Matrix } from './Matrix';
import * as vis from 'vis';
import * as CanvasJS from 'canvasjs';

function ToEdges(adjensceMatrix: Matrix) {
    var nodeNeighbors = adjensceMatrix.GetNodeNeighbors();
    var edges = new Array();
    for (var i = 0; i < nodeNeighbors.length; i++) {
        for (var j = 0; j < nodeNeighbors[i].neighbors.length; j++) {
            console.log(nodeNeighbors[i].neighbors[j].edgeValue );
            edges.push({ from: nodeNeighbors[i].id, to: nodeNeighbors[i].neighbors[j].num, width: nodeNeighbors[i].neighbors[j].edgeValue })
            edges.push({ from: nodeNeighbors[i].neighbors[j].num, to: nodeNeighbors[i].id, width: nodeNeighbors[i].neighbors[j].edgeValue })
        }
    }
    var result = new vis.DataSet(edges);
    return result
}
function ToNode(adjacencyMatrix: Matrix) {
    var nodes = [];
    for (var i = 0; i < adjacencyMatrix.elements.length; i++) {
        nodes.push({ id: i, label: "" + i })
    }
    var result = new vis.DataSet(nodes);
    return result
}
function Graph(adjensceMatrix: Matrix) {
    // create a network
    var container = document.getElementById("graphNetwork");
    //generate edges and nodes
    // provide the data in the vis format
    var data = {
        nodes: ToNode(adjensceMatrix),
        edges: ToEdges(adjensceMatrix)
    };
    //todo Basic network display
    var o = {
        autoResize: true,
        height: '100%',
        width: '100%',
        clickToUse: false,
        // edges: {
        //     
        // },
        edges: {
            color: {
                color: 'black'
            },
            font: '12px arial #ff0000',
            scaling: {
                label: true,
            },
            shadow: false,
            smooth: {
                type: "vertical",
                forceDirection: "none",
                roundness: 0.0,
                enabled: true
            }
        },
        physics: {
            hierarchicalRepulsion: {
                centralGravity: 0
            },
            minVelocity: .001,
            solver: "hierarchicalRepulsion"
        },
        nodes: {
            borderWidth: 1,
            borderWidthSelected: 2,
            chosen: true,
            color: {
                border: 'green',
                background: 'white',
                highlight: {
                    border: '#2B7CE9',
                    background: '#D2E5FF'
                },
                hover: {
                    border: '#2B7CE9',
                    background: '#D2E5FF'
                }
            }
        }
    };
    // initialize network!
    var network = new vis.Network(container, data, o);
    // network.setOptions(o);
    return network;
}

//init
$(document).ready(function () {
    var probability = 0.4;
    var nodeCount = 6;
    var adjensceMatrix = new Matrix(nodeCount, probability);
    var network = Graph(adjensceMatrix);
    adjensceMatrix.DepthFirstSearch();
    document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(4);
    document.getElementById("nodeCount").textContent = ("Node Count: ") + nodeCount;
    $('#generate').click(function (e) {
        e.preventDefault();
        adjensceMatrix = new Matrix(nodeCount, probability);
        network = Graph(adjensceMatrix);
        adjensceMatrix.DepthFirstSearch();
    });
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