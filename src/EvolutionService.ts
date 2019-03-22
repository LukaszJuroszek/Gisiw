import { PopulationModel } from './PopulationModel';
import { ChromosomeModel } from './ChromosomeModel';
import { Matrix } from './Matrix';

export class EvolutionService {
    private _numberOfTournamentRounds: number;
    constructor(private _populationModel: PopulationModel, private _matrix: Matrix, numberOfTournamentRounds: number, private logDebug: boolean) {
        this._numberOfTournamentRounds = numberOfTournamentRounds;
    }

    public runIteration(): PopulationModel {
        var bestCollectionByF1 = new Array<ChromosomeModel>();
        var bestCollectionByF2 = new Array<ChromosomeModel>();

        bestCollectionByF1 = this.getBestChromosomeModelsBy(true);
        bestCollectionByF2 = this.getBestChromosomeModelsBy(false);
        //copy new population to model
        this._populationModel.popuation = new Set(bestCollectionByF1.concat(bestCollectionByF2));

        return this._populationModel;
    }

    public getBestChromosomeModelsBy(by: boolean): Array<ChromosomeModel> {
        var halfOfElementsCount: number = Math.floor(this._populationModel.getPopulationCount() / 2.0);
        var result = new Set<ChromosomeModel>();

        //if by F1 factor
        if (by) {
            if (this.logDebug)
                console.log("--------------- Trunament by F1 started.");
            for (let i = 0; i < this._numberOfTournamentRounds; i++) {
                var { left, rigth } = this.getRandomFirstHalfNumber(halfOfElementsCount);
                var { leftChromosome, rigthChromosome } = this.getLeftAndRigthChromomosomeByNumber(left, rigth);

                result.add(this.selectBestBy(leftChromosome, rigthChromosome, true));
            }
            //else by F2 factor
        } else {
            if (this.logDebug)
                console.log("--------------- Trunament by F2 started.");
            for (let i = 0; i < this._numberOfTournamentRounds; i++) {
                var { left, rigth } = this.getRandomSecondHalfNumber(left, halfOfElementsCount, rigth);
                var { leftChromosome, rigthChromosome } = this.getLeftAndRigthChromomosomeByNumber(left, rigth);

                result.add(this.selectBestBy(leftChromosome, rigthChromosome, false));
            }
        }
        if (result.size == this._numberOfTournamentRounds) {

        } else {
            if (this.logDebug)
                console.log("Duplicate in population deleted, should be: " + this._numberOfTournamentRounds + ", but was: " + result.size + ".");
            var res = this.fixPopulation(result, this._numberOfTournamentRounds);
        }
        if (this.logDebug)
            console.log("--------------- Trunament ended.");

        return Array.from(result.values());
    }

    private fixPopulation(result: Set<ChromosomeModel>, chromosomeCount: number): Set<ChromosomeModel> {
        do {
            var diff: number = Math.abs(chromosomeCount - result.size);
            var missingChromosomes = this._populationModel.generatePopulation(diff);

            if (this.logDebug)
                console.log("Fixing population, missing: " + diff + ".");

            missingChromosomes.forEach(missingElement => {
                result.add(missingElement);
            });
        } while (result.size < chromosomeCount)
        if (this.logDebug)
            console.log(result.size == chromosomeCount ? "ok, " + result.size + " elements in population" : "erron during fixing, was: " + result.size + " should be: " + chromosomeCount)
        return result;
    }

    private getRandomSecondHalfNumber(left: number, halfOfElementsCount: number, rigth: number) {
        var left = this.getSecondHalfIndexNumber(halfOfElementsCount);
        var rigth = this.getSecondHalfIndexNumber(halfOfElementsCount);
        return { left, rigth };
    }

    private getRandomFirstHalfNumber(halfOfElementsCount: number) {
        var left = this.getFirstHalfIndexNumber(halfOfElementsCount);
        var rigth = this.getFirstHalfIndexNumber(halfOfElementsCount);
        return { left, rigth };
    }

    private getLeftAndRigthChromomosomeByNumber(left: number, rigth: number) {
        var leftChromosome = this._populationModel.getChromosomeByIndex(left);
        var rigthChromosome = this._populationModel.getChromosomeByIndex(rigth);
        return { leftChromosome, rigthChromosome };
    }

    private getFirstHalfIndexNumber(halfOfElementsCount: number) {
        return Math.floor((Math.random() * halfOfElementsCount));
    }

    private getSecondHalfIndexNumber(halfOfElementsCount: number) {
        return Math.floor((Math.random() * halfOfElementsCount) + halfOfElementsCount);
    }

    private selectBestBy(leftChromosomeModel: ChromosomeModel, rigthChromosomeModel: ChromosomeModel, by: boolean): ChromosomeModel {
        var leftResult: number = 0;
        var rightResult: number = 0;
        //if by F1 factor
        if (by) {
            leftResult = this._populationModel.getConnectedEdgeCountAndWegithCount(leftChromosomeModel, this._matrix)[0];
            rightResult = this._populationModel.getConnectedEdgeCountAndWegithCount(rigthChromosomeModel, this._matrix)[0];
            //else by F2 factor
        } else {
            leftResult = this._populationModel.getConnectedEdgeCountAndWegithCount(leftChromosomeModel, this._matrix)[1];
            rightResult = this._populationModel.getConnectedEdgeCountAndWegithCount(rigthChromosomeModel, this._matrix)[1];
        }
        return leftResult < rightResult ? leftChromosomeModel : rigthChromosomeModel;
    }

}
