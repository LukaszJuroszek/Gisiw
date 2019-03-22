import { ChromosomeElement, ChromosomeModel } from "./ChromosomeModel";
import { Matrix } from "./Matrix";

export class PopulationModel {
    private _popuation: Set<ChromosomeModel>;

    constructor(private _matrix: Matrix,
        private _populationSize: number,
        private _probability: number,
        private _maxDiffBetweenEdges: number,
        private _maxDiffBetweenNode: number) {

        this._popuation = new Set<ChromosomeModel>();

        this._popuation = this.generatePopulation( _populationSize);
    }

    public generatePopulation(_populationSize: number): Set<ChromosomeModel> {
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
        } while (
            this.getConnectedEdgeCountAndWegithCount(result, matrix)[0] <= this._maxDiffBetweenEdges &&
            this.getNodeDifference(result) <= this._maxDiffBetweenNode);

        return result;
    }

    private generateChromosome(nodeNumbers: number): ChromosomeModel {
        var result: ChromosomeModel = new ChromosomeModel();

        for (let i = 0; i < nodeNumbers; i++) {
            result.chromosome.push(new ChromosomeElement(i, Math.random() < this._probability ? true : false));
        }
        return result;
    }


    public getF1Sum(matrix: Matrix): number {
        var result: number = 0;
        this._popuation.forEach(chromosomeModel => {
            result += this.getConnectedEdgeCountAndWegithCount(chromosomeModel, matrix)[0];
        });
        return result;
    }

    public getF2Sum(matrix: Matrix): number {
        var result: number = 0;
        this._popuation.forEach(chromosomeModel => {
            result += this.getConnectedEdgeCountAndWegithCount(chromosomeModel, matrix)[1];
        });
        return result;
    }

    private getNodeDifference(chromosomeModel: ChromosomeModel): number {
        var firstPartOfGraph = chromosomeModel.chromosome.filter(node => node.isFirstPart === true).length;
        return Math.abs(firstPartOfGraph - chromosomeModel.chromosome.length);
    }

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
        return this._popuation.size;
    }

    public getChromosomeByIndex(index: number): ChromosomeModel {
        return Array.from(this._popuation.values())[index];
    }
}