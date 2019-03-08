import * as vis from 'vis';
import { Matrix } from './Matrix';

function CreateSplitedGraphChromononeFromRow(array, maximumDiffrentBetweenSplitCount) {
    // var numberOfNodes = array.length;
    // var tempArray = new Array(numberOfNodes);
    // for (let i = 0; i < numberOfNodes; i++) {
    //     tempArray[i] = Math.floor(Math.random() * 2);
    // }
    // var sum = tempArray.reduce((x, a) => x + a);
    // if (Math.abs(numberOfNodes - sum) >= maximumDiffrentBetweenSplitCount) {
    //     console.log("Incosistent number of chromosome values");
    //     console.log(Math.abs(numberOfNodes - sum));
    // } else {
    //     console.log("ok");
    // }
    // console.log("firsst part " + sum);
    // console.log("second  part " + Math.abs(numberOfNodes - sum));
    // return new Chromosome(tempArray);
}

function GetNodeNeighbors(adjacencyMatrix) {
    var result = [];
    for (var i = 0; i < adjacencyMatrix.length; i++) {
        var tmp = [];
        for (var j = (i + 1); j < adjacencyMatrix[i].elements.length; j++) {
            if (adjacencyMatrix[i].elements[j] == 1) {
                tmp.push(j);
            }
        }
        tmp.sort((function (a, b) { return b - a }));
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
        tmp.sort((function (a, b) { return b - a }));
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
function GetNodesDegree(network) {
    var nodesDegree = [];
    var tempArray = [];
    network.body.data.edges.forEach(function (edge) {
        tempArray.push(edge);
    }, this);
    for (var index = 0; index < network.body.data.nodes.length; index++) {
        nodesDegree[index] = tempArray.filter(function (e) {
            return e.from == index;
        }).length;
    }
    return nodesDegree
}

function ToEdges(adjensceMatrix: Matrix) {
    var nodeNeighbors = GetNodeNeighbors(adjensceMatrix);
    var edges = [];
    for (var i = 0; i < nodeNeighbors.length; i++) {
        for (var j = 0; j < nodeNeighbors[i].neighbors.length; j++) {
            edges.push({ from: nodeNeighbors[i].id, to: nodeNeighbors[i].neighbors[j] })
            edges.push({ from: nodeNeighbors[i].neighbors[j], to: nodeNeighbors[i].id })
        }
    }
    var result = new vis.DataSet(edges);
    return result
}
function ToAdjensceMatrix(network) {
    // //init matrix
    // var tempArray = [];
    // for (var index = 0; index < network.body.data.nodes.length; index++) {
    //     tempArray[index] = [];
    //     for (var j = 0; j < network.body.data.nodes.length; j++) {
    //         tempArray[index][j] = 0;
    //     }
    // }
    // network.body.data.edges.forEach(function (edge) {
    //     tempArray[edge.from][edge.to] = 1;
    //     tempArray[edge.to][edge.from] = 1;
    // }, this);
    // var result = new Array();
    // for (var s = 0; s < network.body.data.nodes.length; s++) {
    //     result.push(new MatrixRow(tempArray[s], s));
    // }
    // return result
}
function GreedyNodeColoring(network) {
    //this function take only two way directed graph
    var result = [];
    result[0] = { node: 0, color: 0 };
    for (var u = 1; u < network.body.data.nodes.length; u++)
        result[u] = { node: u, color: -1 };  // no color is assigned to u
    var available = [];
    for (var p = 0; p < network.body.data.nodes.length; p++)
        available[p] = false;
    // Assign colors to remaining V-1 vertices
    for (var u = 1; u < network.body.data.nodes.length; u++) {
        // convert edges data set to array and filter current node, u = numer of node
        var filtered = [];
        network.body.data.edges.forEach(function (edge) {
            if (edge.from == u) {
                filtered.push(edge);
            }
        }, this);
        for (var p = 0; p < filtered.length; p++) {
            if (result[filtered[p].to].color != -1) {
                available[result[filtered[p].to].color] = true;
            }
        }
        // Find the first available color
        var cr;
        for (cr = 0; cr < network.body.data.nodes.length; cr++) {
            if (available[cr] == false)
                break;
        }
        result[u].color = cr; // Assign the found color
        // Reset the values back to false for the next iteration
        for (var p = 0; p < filtered.length; p++) {
            if (result[filtered[p].to].color != -1) {
                available[result[filtered[p].to].color] = false;
            }
        }
    }
    return result;
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
    //todo Basic network display
    var options = {
        autoResize: true,
        height: '100%',
        width: '100%',
        clickToUse: false,
        edges: {
            smooth: {
                "type": "vertical",
                "forceDirection": "none",
                "roundness": 0
            },
            color: {
                color: 'black'
            }
        },
        physics: {
            "hierarchicalRepulsion": {
                "centralGravity": 0
            },
            "minVelocity": .001,
            "solver": "hierarchicalRepulsion"
        },
        nodes: {
            borderWidth: 1,
            borderWidthSelected: 2,
            brokenImage: undefined,
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
    }
    // initialize your network!
    var network = new vis.Network(container, data);
    return network;
}
function GetRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
document.getElementById("generate").addEventListener("click", function (e) {
    adjensceMatrix = new Matrix(nodeCount, probability);
    network = Graph(adjensceMatrix);
    DepthFirstSearch(GetNodeNeighborsDFS(adjensceMatrix), nodeCount);
    CreateSplitedGraphChromononeFromRow(adjensceMatrix, maximumDiffrentBetweenSplitCount);
}, false);

//init
var probability = 0.3;
var range = 0.1;
var nodeCount = 6;
var maximumDiffrentBetweenSplitCount = 5;
var adjensceMatrix = new Matrix(nodeCount, probability);
var network = Graph(adjensceMatrix);
DepthFirstSearch(GetNodeNeighborsDFS(adjensceMatrix), nodeCount);
document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(4);
document.getElementById("nodeCount").textContent = ("Node Count: ") + nodeCount;
CreateSplitedGraphChromononeFromRow(adjensceMatrix, maximumDiffrentBetweenSplitCount);