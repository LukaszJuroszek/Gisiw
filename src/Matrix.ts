import { IMatrixElement, MatrixElement } from './MatrixElement';

export interface IMatrix {
    elements: IMatrixElement[][];
}

export class Matrix implements IMatrix {
    elements: IMatrixElement[][];

    constructor(node: number, probabilityForEdge: number) {
        this.GenerateAdjensceMatrix(node, probabilityForEdge);
    }

    GenerateAdjensceMatrix(node: number, probabilityForEdge: number): void {
        this.elements = new MatrixElement[node][node]
        for (var i = 0; i < node; i++) {
            for (var j = (i + 1); j < node; j++) {
                var hasEdge = Math.random() > probabilityForEdge ? 1 : 0;
                this.elements[i][j] = new MatrixElement(hasEdge, i, j);
                this.elements[j][i] = new MatrixElement(0, j, i);
            }
        }
    }
}
