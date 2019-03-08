"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var MatrixElement_1 = require("./MatrixElement");
var Matrix = /** @class */ (function () {
    function Matrix(node, probabilityForEdge) {
        this.GenerateAdjensceMatrix(node, probabilityForEdge);
    }
    Matrix.prototype.GenerateAdjensceMatrix = function (node, probabilityForEdge) {
        this.elements = new MatrixElement_1.MatrixElement[node][node];
        for (var i = 0; i < node; i++) {
            for (var j = (i + 1); j < node; j++) {
                var hasEdge = Math.random() > probabilityForEdge ? 1 : 0;
                this.elements[i][j] = new MatrixElement_1.MatrixElement(hasEdge, i, j);
                this.elements[j][i] = new MatrixElement_1.MatrixElement(0, j, i);
            }
        }
    };
    return Matrix;
}());
exports.Matrix = Matrix;
