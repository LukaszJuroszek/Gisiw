"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var MatrixElement = /** @class */ (function () {
    function MatrixElement(hasEdge, colNumber, rowNumber) {
        this.value = hasEdge;
        this.row = rowNumber;
        this.col = colNumber;
    }
    return MatrixElement;
}());
exports.MatrixElement = MatrixElement;
