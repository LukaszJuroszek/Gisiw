import { ChromosomeModel, ChromosomeElement } from "./ChromosomeModel";
import { Matrix } from "./Matrix";

export class PopulationService {
    //0 - first part, 1 - secondp part
    private _chromosomeParts: Array<number> = [0, 1];

    constructor(private _matrix: Matrix, private _debug: boolean) {
    }

    public initializePopulation(populationSize: number, matrix: Matrix, probability: number, maxDiffBetweenNode: number): Array<ChromosomeModel> {
        var result = new Set<ChromosomeModel>();

        if (result.size < populationSize) {
            do {
                result.add(this.generateChromosome(matrix, probability, maxDiffBetweenNode));
            }
            while (result.size < populationSize);
        }

        return Array.from(result);
    }


    public generateChromosome(matrix: Matrix, probability: number, maxDiffBetweenNode: number): ChromosomeModel {
        var result: ChromosomeModel;
        do {
            result = new ChromosomeModel();
            for (let i = 0; i < matrix.elements.length; i++) {
                if (Math.random() < probability) {
                    result.chromosome.push(new ChromosomeElement(i, this._chromosomeParts[0]));
                } else {
                    result.chromosome.push(new ChromosomeElement(i, this._chromosomeParts[1]));
                }
            }
            result = this.setSumOfF1AndF2(result);

        } while (!this.isNodeCountValid(result, maxDiffBetweenNode))

        return result;
    }

    public generateChromosomes(numberOfChromosomeToGenerate: number, matrix: Matrix, probability: number, maxDiffBetweenNode: number): Array<ChromosomeModel> {

        var result: Set<ChromosomeModel> = new Set<ChromosomeModel>();
        do {
            result.add(this.generateChromosome(matrix, probability, maxDiffBetweenNode));
        } while (result.size == numberOfChromosomeToGenerate)

        return Array.from(result);
    }

    public setSumOfF1AndF2(chromosome: ChromosomeModel): ChromosomeModel {
        var connectedEdgeCountAndWegithCount = this.getConnectedEdgeCountAndWegithCount(chromosome);
        chromosome.sumOfF1 = connectedEdgeCountAndWegithCount[0];
        chromosome.sumOfF2 = connectedEdgeCountAndWegithCount[1];
        return chromosome;
    }

    public isNodeCountValid(chromosomeModel: ChromosomeModel, maxDiffBetweenNode: number) {
        var [firstPartSum, secondPartSum] = this.getNodeChromosomePartCount(chromosomeModel);
        if (this._debug) {
            console.log("firstPartSum: " + firstPartSum);
            console.log("secondPartSum: " + secondPartSum);
            console.log("Math.abs(firstPartSum - secondPartSum): " + Math.abs(firstPartSum - secondPartSum));
            console.log("Math.abs(firstPartSum - secondPartSum) <= maxDiffBetweenNode: " + (Math.abs(firstPartSum - secondPartSum) <= maxDiffBetweenNode));
        }
        return (Math.abs(firstPartSum - secondPartSum) <= maxDiffBetweenNode);
    }

    public getNodeChromosomePartCount(chromosomeModel: ChromosomeModel): [number, number] {
        var nodeSum: number = 0;
        for (let i = 0; i < chromosomeModel.chromosome.length; i++) {
            var isFirstPart = chromosomeModel.chromosome[i].chromosomePartNumber == this._chromosomeParts[0];
            nodeSum = isFirstPart ? nodeSum + 1 : nodeSum;
        }
        var firstPartSum = chromosomeModel.chromosome.length - nodeSum;
        var secondPartSum = chromosomeModel.chromosome.length - firstPartSum;

        return [firstPartSum, secondPartSum];
    }

    public getConnectedEdgeCountAndWegithCount(chromosomeModel: ChromosomeModel): [number, number] {
        var edgeCount: number = 0;
        var edgeWeigthCount: number = 0;
        for (let i = 0; i < this._matrix.elements.length; i++) {
            var rowElem = chromosomeModel.chromosome.find(this.getPartOfGraphBy(i, this._chromosomeParts[0]));
            //first part is selected by user, second is not present, via row
            if (rowElem === undefined) {
                for (let j = 0; j < this._matrix.elements[i].length; j++) {
                    //check if column is in first present
                    var element = chromosomeModel.chromosome.find(this.getPartOfGraphBy(j, this._chromosomeParts[1]));
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