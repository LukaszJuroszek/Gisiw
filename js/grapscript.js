function MatrixRow(matrixRow, index) {
    this.headerIndex = index;
    this.elements = matrixRow;
    return this;
}
function ShowMatrixRow(matrixRow) {
    console.log(
        " index " +
        matrixRow.headerIndex +
        " rows: " +
        matrixRow.elements
    );
}
function RandomNumberByProbabilityStack(probabilityStack) {
    var result = 0;
    var diceRoll = Math.random();
    var cumulative = 0.0;
    for (var i = 0; i < probabilityStack.get.length; i++) {
        cumulative += probabilityStack.get[i].value;
        if (diceRoll < cumulative) {
            result = probabilityStack.get[i].key;
            break;
        }
    }
    if (result === undefined) {
        console.warn("undefined");
    }
    return result;
}
function GenerateAdjensceMatrix(node, probablityForOne, isDirectedGraph) {
    var result = new Array();
    var pbstack = new ProbabilityStack(probablityForOne);
    var tempArray = new Array(node);
    for (var i = 0; i < node; i++) {
        tempArray[i] = [];
        for (var j = 0; j < node; j++) {
            tempArray[i][j] = 0
        }
    }
    for (var i = 0; i < node; i++) {
        for (var j = (i + 1); j < node; j++) {
            var tmp = RandomNumberByProbabilityStack(pbstack);
            tempArray[i][j] = tmp;
            if (isDirectedGraph) {
                tempArray[j][i] = 0;
            }
        }
        result.push(new MatrixRow(tempArray[i], i));
    }
    return result;
}
function ProbabilityStack(forOne) {
    if (forOne < 0 && forOne > 1) {
        console.log("Error In ProbabilityStack " + forOne + "is invalid");
    } else {
        this.get = [{ value: 1 - forOne, key: 0 }, { value: forOne, key: 1 }];
    }
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
function ToEdges(adjensceMatrix, isDirectedGraph) {
    var nodeNeighbors = GetNodeNeighbors(adjensceMatrix);
    var edges = [];
    for (var i = 0; i < nodeNeighbors.length; i++) {
        for (var j = 0; j < nodeNeighbors[i].neighbors.length; j++) {
            edges.push({ from: nodeNeighbors[i].id, to: nodeNeighbors[i].neighbors[j] })
            if (isDirectedGraph) {
                edges.push({ from: nodeNeighbors[i].neighbors[j], to: nodeNeighbors[i].id })
            }
        }
    }
    var result = new vis.DataSet(edges);
    return result
}
function ToAdjensceMatrix(network, isDirectedGraph) {
    //init matrix
    var tempArray = [];
    for (var index = 0; index < network.body.data.nodes.length; index++) {
        tempArray[index] = [];
        for (var j = 0; j < network.body.data.nodes.length; j++) {
            tempArray[index][j] = 0;
        }
    }
    network.body.data.edges.forEach(function (edge) {
        tempArray[edge.from][edge.to] = 1;
        if (isDirectedGraph) {
            tempArray[edge.to][edge.from] = 1;
        } else {
            tempArray[edge.to][edge.from] = 0;
        }
    }, this);
    var result = new Array();
    for (var s = 0; s < network.body.data.nodes.length; s++) {
        result.push(new MatrixRow(tempArray[s], s));
    }
    return result
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
function ToNode(adjacencyMatrix) {
    var nodes = [];
    for (var i = 0; i < adjacencyMatrix.length; i++) {
        nodes.push({ id: i, label: "" + i })
    }
    var result = new vis.DataSet(nodes);
    return result
}
function Graph(adjensceMatrix, isDirectedGraph) {
    //generate edges and nodes
    var edges = ToEdges(adjensceMatrix, isDirectedGraph);
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
        },
        manipulation: {
            addEdge: function (edgeData, callback) {
                if (edgeData.from === edgeData.to) {
                    var r = confirm("Do you want to connect the node to itself?");
                    if (r === true) {
                        callback(edgeData);
                        TableCreate(ToAdjensceMatrix(network, false));
                        DepthFirstSearch(GetNodeNeighborsDFS(ToAdjensceMatrix(network, false)), network.body.data.nodes.length);
                    }
                }
                else {
                    callback(edgeData);
                    TableCreate(ToAdjensceMatrix(network, false));
                    DepthFirstSearch(GetNodeNeighborsDFS(ToAdjensceMatrix(network, false)), network.body.data.nodes.length);
                }
            }
        }
    }
    // initialize your network!
    var network = new vis.Network(container, data, options);
    return network;
}
function TableCreate(adjensceMatrix) {
    if (document.contains(document.getElementById("tableAdj"))) {
        document.getElementById("tableAdj").remove();
    }
    var body = document.getElementById("adjMatrix");
    var tbl = document.createElement("table");
    tbl.setAttribute('id', 'tableAdj');
    tbl.setAttribute('class', 'table-bordered');
    var tbdy = document.createElement("tbody");
    for (var i = 0; i < adjensceMatrix.length; i++) {
        var tr = document.createElement("tr");
        for (var j = 0; j < adjensceMatrix[i].elements.length; j++) {
            var td = document.createElement("td");
            var elementInTd = document.createTextNode(adjensceMatrix[i].elements[j])
            tbl.style.textAlign = "center";
            td.appendChild(elementInTd);
            tr.appendChild(td);
        } tbdy.appendChild(tr);
    } tbl.appendChild(tbdy);
    body.appendChild(tbl);
}
function GetRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
document.getElementById("s1").addEventListener("click", function (e) {
    var adjensceMatrix = GenerateAdjensceMatrix(nodeCount, probability, isDirectedGraph);
    network = Graph(adjensceMatrix, isDirectedGraph);
    TableCreate(adjensceMatrix);
    DepthFirstSearch(GetNodeNeighborsDFS(adjensceMatrix), nodeCount);
}, false);
//init
var probability = 0.5;
var range = 0.1;
var nodeCount = 5;
var isDirectedGraph = true
var adjensceMatrix = GenerateAdjensceMatrix(nodeCount, probability, isDirectedGraph);
var network = Graph(adjensceMatrix, isDirectedGraph);
TableCreate(adjensceMatrix);
DepthFirstSearch(GetNodeNeighborsDFS(adjensceMatrix), nodeCount);
document.getElementById("probability").textContent = ("Probability: ") + (Math.round(probability * 100) / 100).toFixed(4);
document.getElementById("nodeCount").textContent = ("Node Count: ") + nodeCount;
