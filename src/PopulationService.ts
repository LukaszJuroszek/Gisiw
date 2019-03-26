import { ChromosomeModel, ChromosomeElement } from "./ChromosomeModel";
import { Matrix } from "./Matrix";

export class PopulationService {
    private _chromosomeParts: Array<[number, string]> = [[0, "firstPart"], [1, "secondPart"]];

    constructor(private _matrix: Matrix,
        private _probability: number,
        private _maxDiffBetweenEdges: number,
        private _maxDiffBetweenNode: number,
        private _debug: boolean) {
    }

    public generatePopulationOrAddMissingIfPopulationSize(result: Array<ChromosomeModel>, populationSize: number): Array<ChromosomeModel> {
        var temp = new Set<ChromosomeModel>(result);
        if (temp.size < populationSize) {
            do {
                temp.add(this.generateChromosome(this._matrix));
            }
            while (temp.size < populationSize);
        }

        result = Array.from(temp);
        return result;
    }

    public generateChromosome(matrix: Matrix): ChromosomeModel {
        var result: ChromosomeModel = new ChromosomeModel();
        do {
            result = new ChromosomeModel();
            for (let i = 0; i < matrix.elements.length; i++) {
                if (Math.random() < this._probability) {
                    result.chromosome.push(new ChromosomeElement(i, this._chromosomeParts[0][0]));
                } else {
                    result.chromosome.push(new ChromosomeElement(i, this._chromosomeParts[1][0]));
                }
            }
            var connectedEdgeCountAndWegithCount = this.getConnectedEdgeCountAndWegithCount(result);
            result.sumOfF1 = connectedEdgeCountAndWegithCount[0];
            result.sumOfF2 = connectedEdgeCountAndWegithCount[1];
            
        } while (!this.isChromosomeValid(result))

        return result;
    }

    public isChromosomeValid(chrmomosome: ChromosomeModel): boolean {
        return this.isEdgeCountValid(chrmomosome) && this.isNodeCountValid(chrmomosome);
    }

    public isEdgeCountValid(chrmomosome: ChromosomeModel): boolean {
        return true
    }

    public isNodeCountValid(chromosomeModel: ChromosomeModel) {
        var nodeSum: number = 0;
        for (let i = 0; i < chromosomeModel.chromosome.length; i++) {
            var isFirstPart = chromosomeModel.chromosome[i].chromosomePartNumber === this._chromosomeParts[0][0];
            nodeSum = isFirstPart ? nodeSum + 1 : nodeSum;
        }
        var firstPartSum = chromosomeModel.chromosome.length - nodeSum;
        var secondPartSum = chromosomeModel.chromosome.length - firstPartSum;
        return (Math.abs(firstPartSum - secondPartSum) <= this._maxDiffBetweenNode);
    }

    public getConnectedEdgeCountAndWegithCount(chromosomeModel: ChromosomeModel): [number, number] {
        var edgeCount: number = 0;
        var edgeWeigthCount: number = 0;
        for (let i = 0; i < this._matrix.elements.length; i++) {
            var rowElem = chromosomeModel.chromosome.find(this.getPartOfGraphBy(i, this._chromosomeParts[0][0]));
            //first part is selected by user, second is not present, via row
            if (rowElem === undefined) {
                for (let j = 0; j < this._matrix.elements[i].length; j++) {
                    //check if column is in first present
                    var element = chromosomeModel.chromosome.find(this.getPartOfGraphBy(j, this._chromosomeParts[1][0]));
                    if (element !== undefined && this._matrix.elements[i][j] >= 1) {
                        edgeCount++;
                        edgeWeigthCount += this._matrix.elements[i][j];
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

    public getF1SumF2SumAndParetoPairs(population: Array<ChromosomeModel>): [number, number, Array<[number, number]>] {
        var sumf1: number = 0;
        var sumf2: number = 0;
        var pairs: Array<[number, number]> = new Array<[number, number]>();
        population.forEach(chromosomeModel => {
            sumf1 += chromosomeModel.sumOfF1;
            sumf2 += chromosomeModel.sumOfF2;
            pairs.push([chromosomeModel.sumOfF1, chromosomeModel.sumOfF2]);
        });
        return [sumf1, sumf2, pairs];
    }

    public setStatusString(statusString: string) {
        document.getElementById("currentStatus").textContent = "Current Status: " + statusString;
    }
}