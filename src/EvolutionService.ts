import { Matrix } from './Matrix';
import { EvolutionModel, ChromosomeElement } from './EvolutionModel';

export interface IEvolutionService {
}

export class EvolutionService implements IEvolutionService {
    private maxDiffBeetweanEdges: number = 6;
    private probability: number = 0.4;
    constructor(private _matrix: Matrix) {

    }

    public CreateEvolutionModelFromMatrix(): EvolutionModel {
        var result = new EvolutionModel();
        do {
            var result = this.GenerateEvolutionModel(this._matrix);
            console.log("GeneratingEvM!");
        } while (Math.abs(this.GetConnectedEdgesCount(result)) <= this.maxDiffBeetweanEdges);

        return result;
    }

    private GenerateEvolutionModel(matrix: Matrix) {
        var result = new EvolutionModel();
        for (let i = 0; i < matrix.elements.length; i++) {
            var isFirstPart = Math.random() < this.probability ? true : false;
            result.chromosome.push(new ChromosomeElement(i, isFirstPart));
        }
        return result;
    }

    GetConnectedEdgesCount(evolutionModel: EvolutionModel): number {
        var edgeSum: number = 0;
        for (let i = 0; i < this._matrix.elements.length; i++) {
            //first part is selected by user, second is not present, via row
            var rowElem = evolutionModel.chromosome.find(n => {
                return n.isFirstPart === false && n.nodeNumber === i;
            });

            if (rowElem == undefined) {
                for (let j = 0; j < this._matrix.elements[i].length; j++) {
                    //check if column is in first present
                    var element = evolutionModel.chromosome.find(n => {
                        return n.isFirstPart === true && n.nodeNumber === j;
                    });
                    if (element !== undefined && this._matrix.elements[i][j].value >= 1) {
                        edgeSum++;
                    }
                }
            }
        }
        return edgeSum;
    }
}
