import { Matrix } from "./Matrix";
import * as vis from "vis";

export class GraphService {
    private _adjensceMatrix: Matrix;
    constructor(adjensceMatrix: Matrix) {
        this._adjensceMatrix = adjensceMatrix;
        this.CreateGraph();
    }
    public ToEdges(adjensceMatrix: Matrix) {
        var nodeNeighbors = adjensceMatrix.GetNodeNeighbors();
        var edges = new Array();
        for (var i = 0; i < nodeNeighbors.length; i++) {
            for (var j = 0; j < nodeNeighbors[i].neighbors.length; j++) {
                edges.push({ from: nodeNeighbors[i].id, to: nodeNeighbors[i].neighbors[j].num, width: nodeNeighbors[i].neighbors[j].edgeValue / 10.0 })
                edges.push({ from: nodeNeighbors[i].neighbors[j].num, to: nodeNeighbors[i].id, width: nodeNeighbors[i].neighbors[j].edgeValue / 10.0 })
            }
        }
        var result = new vis.DataSet(edges);
        return result
    }

    public ToNode(adjacencyMatrix: Matrix) {
        var nodes = [];
        for (var i = 0; i < adjacencyMatrix.elements.length; i++) {
            nodes.push({ id: i, label: "" + i })
        }
        var result = new vis.DataSet(nodes);
        return result
    }

    public CreateGraph() {
        // create a network
        var container = document.getElementById("graphNetwork");
        //generate edges and nodes
        // provide the data in the vis format
        var data = {
            nodes: this.ToNode(this._adjensceMatrix),
            edges: this.ToEdges(this._adjensceMatrix)
        };
        //todo Basic network display
        var o = {
            autoResize: true,
            height: '100%',
            width: '100%',
            clickToUse: false,
            edges: {
                color: {
                    color: 'blue'
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
                forceAtlas2Based: {
                    gravitationalConstant: -30,
                    centralGravity: 0.005,
                    springLength: 135,
                    springConstant: 0,
                    damping: 1
                  },
                maxVelocity: 40,
                minVelocity: 0.3,
                solver: "forceAtlas2Based",
                timestep: 1
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
}