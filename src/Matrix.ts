
export class Matrix {
    private weigthIfNoEdge: number = 0;
    private defaultWeigthIfHasEdge: number = 1;
    private maxEdgeWeigth: number = 100;
    
    elements: Array<Array<number>>;

    constructor(node: number, probabilityForEdge: number) {
        this.GenerateAdjensceMatrix(node, probabilityForEdge);
    }

    private GenerateAdjensceMatrix(node: number, probabilityForEdge: number): void {
        this.elements = new Array<Array<number>>();
        for (var i = 0; i < node; i++) {
            this.elements[i] = new Array<number>(node);
            for (var j = 0; j < node; j++) {
                this.elements[i][j] = this.weigthIfNoEdge;
            }
        }

        for (var i = 0; i < node; i++) {
            for (var j = (i + 1); j < node; j++) {
                var hasEdge = Math.random() < probabilityForEdge ? true : false;
                var weigthIfHasEdge = this.defaultWeigthIfHasEdge;
                if (hasEdge) {
                    weigthIfHasEdge = Math.floor((Math.random() * this.maxEdgeWeigth) + this.defaultWeigthIfHasEdge);
                    this.elements[i][j] = weigthIfHasEdge;
                } else {
                    this.elements[i][j] = this.weigthIfNoEdge;
                }
                this.elements[j][i] = this.weigthIfNoEdge
            }
        }
    }

    public GetNodeNeighbors() {
        var result = new Array();
        for (var i = 0; i < this.elements.length; i++) {
            var tmp = new Array();
            for (var j = (i + 1); j < this.elements.length; j++) {
                this.elements[j][i] = this.elements[i][j];
            }
        }
        for (var i = 0; i < this.elements.length; i++) {
            var tmp = new Array();
            for (var j = 0; j < this.elements[i].length; j++) {
                if (this.elements[i][j] >= 1) {
                    tmp.push({ num: j, edgeValue: this.elements[i][j] });
                }
            }
            tmp.sort((function (a, b) { return b.num - a.num }));
            result.push({ id: i, neighbors: tmp });
            tmp = new Array();
        }
        return result;
    }

    public DepthFirstSearch(): boolean {
        var nodeNeighbors = this.GetNodeNeighbors();
        var isCostistent: boolean = false;
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
        isCostistent = visited.size === this.elements.length;
        return isCostistent;
    }
}
