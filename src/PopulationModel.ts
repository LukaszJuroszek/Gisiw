import { ChromosomeElement, EvolutionModel } from "./EvolutionModel";
import { Matrix } from "./Matrix";

export class PopulationModel {
    private _popuation: Array<EvolutionModel>;
    private maxDiffBetweenEdges: number = 6;
    private maxDiffBetweenNode: number = 4;
    private probability: number = 0.4;

    constructor(private _matrix: Matrix) {

    }

    public GetNodeDiff(evolutionModel: EvolutionModel): number {
        var firstPartOfGraph = evolutionModel.chromosome.filter(node => node.isFirstPart === true).length;
        return Math.abs(firstPartOfGraph - evolutionModel.chromosome.length);
    }
    private GenerateEvolutionModel(matrix: Matrix) {
        var result = new EvolutionModel();
        for (let i = 0; i < matrix.elements.length; i++) {
            var isFirstPart = Math.random() < this.probability ? true : false;
            result.chromosome.push(new ChromosomeElement(i, isFirstPart));
        }
        return result;
    }

    public CreateEvolutionModelFromMatrix(): EvolutionModel {
        var result = new EvolutionModel();
        do {
            var result = this.GenerateEvolutionModel(this._matrix);
            console.log("GeneratingEvM!");
        } while (
            this.GetConnectedEdgesCount(result) <= this.maxDiffBetweenEdges &&
            this.GetNodeDiff(result) <= this.maxDiffBetweenNode);
        return result;
    }

    public GetF1Sum(): number {
        var result: number = 0;
        this._popuation.forEach(evolutionModel => {
            result += this.GetConnectedEdgesCount(evolutionModel);
        });
        return result;
    }

    public GetF2Sum(): number {
        var result: number = 0;
        
        return result;
    }
    public GetConnectedEdgesCount(evolutionModel: EvolutionModel): number {
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
        return Math.abs(edgeSum);
    }
}