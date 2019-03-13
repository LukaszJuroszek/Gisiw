import { IMatrixElement, MatrixElement } from './MatrixElement';

export interface IMatrix {
    elements: Array<Array<IMatrixElement>>;
}

export class Matrix implements IMatrix {
    elements: Array<Array<IMatrixElement>>;

    constructor(node: number, probabilityForEdge: number) {
        this.GenerateAdjensceMatrix(node, probabilityForEdge);
    }

    private GenerateAdjensceMatrix(node: number, probabilityForEdge: number): void {
        this.elements = new Array<Array<MatrixElement>>();
        for (var i = 0; i < node; i++) {
            this.elements[i] = new Array<MatrixElement>(node);
            for (var j = 0; j < node; j++) {
                this.elements[i][j] = new MatrixElement(0, i, j);
            }
        }

        for (var i = 0; i < node; i++) {
            for (var j = (i + 1); j < node; j++) {
                var hasEdge = Math.random() < probabilityForEdge ? true : false;
                var weigthIfHasEdge = 1;
                if (hasEdge) {
                    weigthIfHasEdge = Math.floor((Math.random() * 10) + 1);
                    this.elements[i][j] = new MatrixElement(weigthIfHasEdge, j, i);
                } else {
                    this.elements[i][j] = new MatrixElement(0, j, i);
                }
                this.elements[j][i] = new MatrixElement(0, i, j);
            }
        }
    }

    public GetNodeNeighbors() {
        var result = new Array();
        for (var i = 0; i < this.elements.length; i++) {
            var tmp = new Array();
            for (var j = (i + 1); j < this.elements.length; j++) {
                this.elements[j][i].value = this.elements[i][j].value;
            }
        }
        for (var i = 0; i < this.elements.length; i++) {
            var tmp = new Array();
            for (var j = 0; j < this.elements[i].length; j++) {
                if (this.elements[i][j].value >= 1) {
                    tmp.push({ num: j, edgeValue: this.elements[i][j].value });
                }
            }
            tmp.sort((function (a, b) { return b.num - a.num }));
            result.push({ id: i, neighbors: tmp });
            tmp = new Array();
        }
        return result;
    }

    public DepthFirstSearch() {
        var nodeNeighbors = this.GetNodeNeighbors();
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
                if (!visited.has(neighbor.num)) {
                    stack.push(neighbor.num);
                }
            }, this);
        }
        document.getElementById("dfsResult").textContent = ("Is consistent: ") + (visited.size === this.elements.length);
    }
}
