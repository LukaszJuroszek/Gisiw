export interface IMatrixElement {
    value: number;
    row: number;
    col: number;
}

export class MatrixElement implements IMatrixElement {
    value: number;
    row: number;
    col: number;

    constructor(hasEdge: number, colNumber:number,rowNumber: number,  ){
        this.value = hasEdge;
        this.row = rowNumber;
        this.col = colNumber;
    }
}