import { Matrix } from "./Matrix";
import * as vis from "vis";
import { ChromosomeModel } from "./chromosomeModel";

export class GraphService {
    private _adjensceMatrix: Matrix;
    constructor(adjensceMatrix: Matrix, private _continerId: string) {
        this._adjensceMatrix = adjensceMatrix;
        var data = {
            nodes: this.matrixToNode(this._adjensceMatrix),
            edges: this.matrixToEdges(this._adjensceMatrix)
        };
        this.createGraph(this._continerId, data, this.getOptions());
    }

    public CreateGraphForBestChromosome(continerId: string, population: Array<ChromosomeModel>, currentBestChromosome: ChromosomeModel): ChromosomeModel {
        var best = this.getBestChromosome(population);
        if (best.getSumOfF1AndF2() < currentBestChromosome.getSumOfF1AndF2()) {
            currentBestChromosome = best;
        }

        var data = {
            nodes: this.chromosomeToNode(currentBestChromosome),
            edges: this.chromosomeToEdges(currentBestChromosome, this._adjensceMatrix)
        };

        this.createGraph(continerId, data, this.getOptionsForBestChromosome());

        return currentBestChromosome;
    }

    public getBestChromosome(population: Array<ChromosomeModel>): ChromosomeModel {
        var currentBest = new ChromosomeModel();

        population.forEach(element => {
            if (element.getSumOfF1AndF2() < currentBest.getSumOfF1AndF2()) {
                currentBest = element;
            }
        });

        return currentBest;
    }
    public createGraph(continerId: string, data: any, options: any) {
        // create a network
        var container = document.getElementById(continerId);
        //generate edges and nodes
        // provide the data in the vis format

        // initialize network!
        var network = new vis.Network(container, data, options);
        // network.setOptions(o);
        return network;
    }

    public matrixToEdges(adjensceMatrix: Matrix) {
        var nodeNeighbors = adjensceMatrix.GetNodeNeighbors();
        var edges = new Array();

        for (var i = 0; i < nodeNeighbors.length; i++) {
            for (var j = 0; j < nodeNeighbors[i].neighbors.length; j++) {
                edges.push({ from: nodeNeighbors[i].id, to: nodeNeighbors[i].neighbors[j].num, label: nodeNeighbors[i].neighbors[j].edgeValue.toString(), font: { align: 'top' } })
                edges.push({ from: nodeNeighbors[i].neighbors[j].num, to: nodeNeighbors[i].id })
            }
        }

        return new vis.DataSet(edges);
    }

    public matrixToNode(adjacencyMatrix: Matrix) {
        var nodes = [];

        for (var i = 0; i < adjacencyMatrix.elements.length; i++) {
            nodes.push({ id: i, label: "" + i })
        }

        return new vis.DataSet(nodes);
    }

    public chromosomeToNode(chromosome: ChromosomeModel) {
        var nodes = [];

        for (var i = 0; i < chromosome.chromosome.length; i++) {
            nodes.push({ id: i, label: "" + i, level: chromosome.chromosome[i].chromosomePartNumber + 1 })
        }
        return new vis.DataSet(nodes);
    }

    public chromosomeToEdges(chromosome: ChromosomeModel, matrix: Matrix) {
        var nodeNeighbors = matrix.GetNodeNeighbors();
        var edges = new Array();

        for (var i = 0; i < nodeNeighbors.length; i++) {
            for (var j = 0; j < nodeNeighbors[i].neighbors.length; j++) {
                edges.push({ from: nodeNeighbors[i].id, to: nodeNeighbors[i].neighbors[j].num, label: nodeNeighbors[i].neighbors[j].edgeValue.toString(), font: { align: 'top' } })
                edges.push({ from: nodeNeighbors[i].neighbors[j].num, to: nodeNeighbors[i].id })
            }
        }

        return new vis.DataSet(edges);
    }

    private getOptionsForBestChromosome() {
        return {
            layout: {
                hierarchical: {
                    enabled: true,
                    levelSeparation: 300,
                    nodeSpacing: 100,
                    blockShifting: true,
                    parentCentralization: true,
                    direction: 'DU',        // UD, DU, LR, RL
                    sortMethod: 'directed'   // hubsize, directed
                }
            },
            autoResize: true,
            height: '100%',
            width: '100%',
            clickToUse: false,
            edges: {
                color: {
                    color: '3E89D5'
                },
                shadow: false,
                smooth: {
                    type: "vertical",
                    forceDirection: "none",
                    roundness: 0.0,
                    enabled: true
                }
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
    }

    private getOptions() {
        return {
            autoResize: true,
            height: '100%',
            width: '100%',
            clickToUse: false,
            edges: {
                color: {
                    color: '3E89D5'
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
    }
}