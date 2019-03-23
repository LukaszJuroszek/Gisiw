import { ChromosomeModel, ChromosomeElement } from "./ChromosomeModel";
import { Matrix } from "./Matrix";

export class PopulationService {
    private _chromosomeParts: Array<[number, string]> = [[0, "firstPart"], [1, "secondPart"]];
    public popuation: Set<ChromosomeModel>;

    constructor(private _matrix: Matrix,
        populationSize: number,
        private _probability: number,
        private _maxDiffBetweenEdges: number,
        private _maxDiffBetweenNode: number,
        private _logDebug: boolean) {

        this.popuation = this.generatePopulation(populationSize);
    }

    public generatePopulation(_populationSize: number): Set<ChromosomeModel> {
        if (this._logDebug)
            console.log("Generate population with " + _populationSize + " elements.");
        var result = new Set<ChromosomeModel>();
        do {
            result.add(this.generateChromosomeBy(this._matrix));
        } while (result.size < _populationSize);

        return result;
    }

    public generateChromosomeBy(matrix: Matrix): ChromosomeModel {
        var result: ChromosomeModel;
        do {
            result = this.generateChromosome(matrix.elements.length);
        } while (result.sumOfF1 <= this._maxDiffBetweenEdges);
        return result;
    }

    private generateChromosome(nodeNumbers: number): ChromosomeModel {
        var result: ChromosomeModel = new ChromosomeModel();
        var nodeNumber = 0;
        do {
            nodeNumber = 0;
            for (let i = 0; i < nodeNumbers; i++) {
                var isFirstPart = Math.random() < this._probability ? true : false;
                if (isFirstPart) {
                    result.chromosome.push(new ChromosomeElement(i, this._chromosomeParts[0][0]));
                } else {
                    result.chromosome.push(new ChromosomeElement(i, this._chromosomeParts[1][0]));
                }
                nodeNumber = isFirstPart ? nodeNumber + 1 : nodeNumber;
            }
        } while (Math.abs(result.chromosome.length - nodeNumber) <= this._maxDiffBetweenNode);

        var we = this.getConnectedEdgeCountAndWegithCount(result, this._matrix);
        result.sumOfF1 = we[0];
        result.sumOfF2 = we[1];

        return result;
    }

    public getConnectedEdgeCountAndWegithCount(chromosomeModel: ChromosomeModel, matrix: Matrix): [number, number] {
        var edgeCount: number = 0;
        var edgeWeigthCount: number = 0;

        for (let i = 0; i < matrix.elements.length; i++) {
            var rowElem = chromosomeModel.chromosome.find(this.getPartOfGraphBy(i, this._chromosomeParts[0][0]));
            //first part is selected by user, second is not present, via row
            if (rowElem === undefined) {
                for (let j = 0; j < matrix.elements[i].length; j++) {
                    //check if column is in first present
                    var element = chromosomeModel.chromosome.find(this.getPartOfGraphBy(j, this._chromosomeParts[1][0]));
                    if (element !== undefined && matrix.elements[i][j] >= 1) {
                        edgeCount++;
                        edgeWeigthCount += matrix.elements[i][j];
                    }
                }
            }
        }

        return [edgeCount, edgeWeigthCount];
    }

    private getPartOfGraphBy(nodeNumber: number, partNumber: number): (value: ChromosomeElement, index: number, obj: ChromosomeElement[]) => boolean {
        return n => {
            return n.chromosomePartNumber === partNumber && n.nodeNumber === nodeNumber;
        };
    }
}