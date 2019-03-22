import { ChromosomeElement, ChromosomeModel } from "./ChromosomeModel";
import { Matrix } from "./Matrix";

export class PopulationModel {
    public popuation: Set<ChromosomeModel>;

    constructor(private _matrix: Matrix,
        private _populationSize: number,
        private _probability: number,
        private _maxDiffBetweenEdges: number,
        private _maxDiffBetweenNode: number,
        private _logDebug: boolean) {

        this.popuation = this.generatePopulation(_populationSize);
    }

    public generatePopulation(_populationSize: number): Set<ChromosomeModel> {
        if (this._logDebug)
            console.log("Generate population with " + _populationSize + " elements.");
        var result = new Set<ChromosomeModel>();
        do {
            result.add(this.createChromosomeFromMatrix(this._matrix));
        } while (result.size < _populationSize);

        return result;
    }

    public createChromosomeFromMatrix(matrix: Matrix): ChromosomeModel {
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
                result.chromosome.push(new ChromosomeElement(i, isFirstPart));
                nodeNumber = isFirstPart ? nodeNumber + 1 : nodeNumber;
            }
        } while (Math.abs(result.chromosome.length - nodeNumber) <= this._maxDiffBetweenNode);

        var we = this.getConnectedEdgeCountAndWegithCount(result, this._matrix);
        result.sumOfF1 = we[0];
        result.sumOfF2 = we[1];

        return result;
    }

    public getF1SumF2SumAndParetoPairs(): [number, number, Array<[number, number]>] {
        var sumf1: number = 0;
        var sumf2: number = 0;
        var pairs: Array<[number, number]> = new Array<[number, number]>();
        this.popuation.forEach(chromosomeModel => {
            console.log(chromosomeModel.getStringWithSums())
            sumf1 += chromosomeModel.sumOfF1;
            sumf2 += chromosomeModel.sumOfF2;
            pairs.push([chromosomeModel.sumOfF1, chromosomeModel.sumOfF2]);
        });
        return [sumf1, sumf2, pairs];
    }

    // public getParetoPairs(): Array<[number, number]> {
    //     var result: Array<[number, number]> = new Array<[number, number]>();
    //     Array.from(this.popuation.values()).forEach(chromosomeModel => {
    //         var points = this.getConnectedEdgeCountAndWegithCount(chromosomeModel, this._matrix);
    //         result.push([points[0], points[1]]);
    //     });
    //     return result;
    // }

    // public getF1Sum(): number {
    //     var result: number = 0;
    //     this.popuation.forEach(chromosomeModel => {
    //         result += this.getConnectedEdgeCountAndWegithCount(chromosomeModel, this._matrix)[0];
    //     });
    //     return result;
    // }

    // public getF2Sum(): number {
    //     var result: number = 0;
    //     this.popuation.forEach(chromosomeModel => {
    //         result += this.getConnectedEdgeCountAndWegithCount(chromosomeModel, this._matrix)[1];
    //     });
    //     return result;
    // }

    public getConnectedEdgeCountAndWegithCount(chromosomeModel: ChromosomeModel, matrix: Matrix): [number, number] {
        var edgeCount: number = 0;
        var edgeWeigthCount: number = 0;

        for (let i = 0; i < matrix.elements.length; i++) {
            var rowElem = chromosomeModel.chromosome.find(this.findFirstPartOfGraph(i));

            //first part is selected by user, second is not present, via row
            if (rowElem === undefined) {
                for (let j = 0; j < matrix.elements[i].length; j++) {
                    //check if column is in first present
                    var element = chromosomeModel.chromosome.find(this.findSecondPartOfGraph(j));
                    if (element !== undefined && matrix.elements[i][j].value >= 1) {
                        edgeCount++;
                        edgeWeigthCount += matrix.elements[i][j].value;
                    }
                }
            }
        }

        return [edgeCount, edgeWeigthCount];
    }

    private findSecondPartOfGraph(j: number): (value: ChromosomeElement, index: number, obj: ChromosomeElement[]) => boolean {
        return n => {
            return n.isFirstPart === true && n.nodeNumber === j;
        };
    }

    private findFirstPartOfGraph(i: number): (value: ChromosomeElement, index: number, obj: ChromosomeElement[]) => boolean {
        return n => {
            return n.isFirstPart === false && n.nodeNumber === i;
        };
    }

    public getPopulationCount(): number {
        return this.popuation.size;
    }

    public getChromosomeByIndex(index: number): ChromosomeModel {
        return Array.from(this.popuation.values())[index];
    }
}