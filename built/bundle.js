(function () {
    'use strict';

    Object.defineProperty(exports, "__esModule", { value: true });
    var vis = require("vis");
    var Matrix_1 = require("./Matrix");
    function GetNodeNeighbors(adjacencyMatrix) {
        var result = [];
        for (var i = 0; i < adjacencyMatrix.length; i++) {
            var tmp = [];
            for (var j = (i + 1); j < adjacencyMatrix[i].elements.length; j++) {
                if (adjacencyMatrix[i].elements[j] == 1) {
                    tmp.push(j);
                }
            }
            tmp.sort((function (a, b) { return b - a; }));
            result.push({ id: i, neighbors: tmp });
            tmp = [];
        }
        return result;
    }
    function GetNodeNeighborsDFS(adjacencyMatrix) {
        var result = [];
        // fix for function (this will simulate two way graph...)
        for (var i = 0; i < adjacencyMatrix.length; i++) {
            var tmp = [];
            for (var j = (i + 1); j < adjacencyMatrix.length; j++) {
                adjacencyMatrix[j].elements[i] = adjacencyMatrix[i].elements[j];
            }
        }
        for (var i = 0; i < adjacencyMatrix.length; i++) {
            var tmp = [];
            for (var j = 0; j < adjacencyMatrix[i].elements.length; j++) {
                if (adjacencyMatrix[i].elements[j] == 1) {
                    tmp.push(j);
                }
            }
            tmp.sort((function (a, b) { return b - a; }));
            result.push({ id: i, neighbors: tmp });
            tmp = [];
        }
        return result;
    }
    function DepthFirstSearch(nodeNeighbors, nodesCount) {
        //this function take only two way directed graph
        var visited = new Set();
        var stack = [];
        stack.push(0);
        while (stack.length > 0) {
            var vertex = stack.pop();
            visited.add(vertex);
            var node = nodeNeighbors.find(function checkId(n) {
                return n.id === vertex;
            });
            node.neighbors.forEach(function (neighbor) {
                if (!visited.has(neighbor)) {
                    stack.push(neighbor);
                }
            }, this);
        }
        document.getElementById("dfsResult").textContent = ("Is consistent: ") + (visited.size === nodesCount);
    }
    function ToEdges(adjensceMatrix) {
        var nodeNeighbors = GetNodeNeighbors(adjensceMatrix);
        var edges = [];
        for (var i = 0; i < nodeNeighbors.length; i++) {
            for (var j = 0; j < nodeNeighbors[i].neighbors.length; j++) {
                edges.push({ from: nodeNeighbors[i].id, to: nodeNeighbors[i].neighbors[j] });
                edges.push({ from: nodeNeighbors[i].neighbors[j], to: nodeNeighbors[i].id });
            }
        }
        var result = new vis.DataSet(edges);
        return result;
    }
    function ToNode(adjacencyMatrix) {
        var nodes = [];
        for (var i = 0; i < adjacencyMatrix.elements.length; i++) {
            nodes.push({ id: i, label: "" + i });
        }
        var result = new vis.DataSet(nodes);
        return result;
    }
    function Graph(adjensceMatrix) {
        //generate edges and nodes
        var edges = ToEdges(adjensceMatrix);
        var nodes = ToNode(adjensceMatrix);
        // create a network
        var container = document.getElementById("graphNetwork");
        // provide the data in the vis format
        var data = {
            nodes: nodes,
            edges: edges
        };
        // initialize your network!
        var network = new vis.Network(container, data);
        return network;
    }
    document.getElementById("generate").addEventListener("click", function (e) {
        adjensceMatrix = new Matrix_1.Matrix(nodeCount, probability);
        network = Graph(adjensceMatrix);
        DepthFirstSearch(GetNodeNeighborsDFS(adjensceMatrix), nodeCount);
    }, false);
    //init
    var probability = 0.3;
    var nodeCount = 6;
    var adjensceMatrix = new Matrix_1.Matrix(nodeCount, probability);
    var network = Graph(adjensceMatrix);
    DepthFirstSearch(GetNodeNeighborsDFS(adjensceMatrix), nodeCount);
    document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(4);
    document.getElementById("nodeCount").textContent = ("Node Count: ") + nodeCount;

}());
