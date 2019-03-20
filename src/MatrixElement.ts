export class MatrixElement {
    value: number;
    row: number;
    col: number;

    constructor(hasEdge: number, colNumber:number,rowNumber: number,  ){
        this.value = hasEdge;
        this.row = rowNumber;
        this.col = colNumber;
    }
}