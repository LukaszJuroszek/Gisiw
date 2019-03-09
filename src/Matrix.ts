import { IMatrixElement, MatrixElement } from './MatrixElement';

export interface IMatrix {
    elements: Array<Array<IMatrixElement>>;
}

export class Matrix implements IMatrix {
    elements: Array<Array<IMatrixElement>>;

    constructor(node: number, probabilityForEdge: number) {
        this.GenerateAdjensceMatrix(node, probabilityForEdge);
    }

    GenerateAdjensceMatrix(node: number, probabilityForEdge: number): void {
        this.elements = new Array<Array<MatrixElement>>();
        for (var i = 0; i < node; i++) {
            this.elements[i] = new Array<MatrixElement>(node);
        }
        for (var i = 0; i < node; i++) {
            for (var j = (i + 1); j < node; j++) {
                var hasEdge = Math.random() > probabilityForEdge ? 1 : 0;
                this.elements[i][j] = new MatrixElement(hasEdge, i, j);
                this.elements[j][i] = new MatrixElement(0, j, i);
            }
        }

        for (var i = 0; i < node; i++) {
            for (var j = (i + 1); j < node; j++) {
                var hasEdge = Math.random() > probabilityForEdge ? 1 : 0;
                this.elements[i][j] = new MatrixElement(hasEdge, i, j);
                this.elements[j][i] = new MatrixElement(0, j, i);
            }
        }
    }
}
