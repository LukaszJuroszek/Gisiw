import { PopulationModel } from './PopulationModel';
import { ChromosomeModel } from './ChromosomeModel';
import { PopulationService } from './PopulationService';

export class EvolutionService {

    constructor(private numberOfTournamentRounds: number,
        private populationService: PopulationService,
        private logDebug: boolean) { }

    public runIteration(population: PopulationModel): PopulationModel {
        console.log(population.getF1SumF2SumAndParetoPairs());

        var bestCollectionByF1 = new Array<ChromosomeModel>();
        var bestCollectionByF2 = new Array<ChromosomeModel>();

        bestCollectionByF1 = this.getBestChromosomeModelsBy(true, population);
        bestCollectionByF2 = this.getBestChromosomeModelsBy(false, population);

        //copy new population to model
        population.popuation = new Set(bestCollectionByF1.concat(bestCollectionByF2));
         population.popuation = this.shuffle(population.popuation);
        
        console.log(population.getF1SumF2SumAndParetoPairs());
        console.log("--------------- _populationModel." + population.popuation.size);
        return population;
    }

    public getBestChromosomeModelsBy(by: boolean, populationModel: PopulationModel): Array<ChromosomeModel> {
        var halfOfElementsCount: number = Math.floor(populationModel.getPopulationCount() / 2.0);
        var result = new PopulationModel(new Set<ChromosomeModel>());
        //if by F1 factor
        if (by) {
            if (this.logDebug)
                console.log("--------------- Trunament by F1 started.");
            for (let i = 0; i < this.numberOfTournamentRounds; i++) {
                var { left, rigth } = this.getRandomFirstHalfNumber(halfOfElementsCount);
                var { leftChromosome, rigthChromosome } = this.getLeftAndRigthChromomosomeByNumber(left, rigth, populationModel);

                result.popuation.add(this.selectBestBy(leftChromosome, rigthChromosome, true));
            }
            //else by F2 factor
        } else {
            if (this.logDebug)
                console.log("--------------- Trunament by F2 started.");
            for (let i = 0; i < this.numberOfTournamentRounds; i++) {
                var { left, rigth } = this.getRandomSecondHalfNumber(left, halfOfElementsCount, rigth);
                var { leftChromosome, rigthChromosome } = this.getLeftAndRigthChromomosomeByNumber(left, rigth, populationModel);

                result.popuation.add(this.selectBestBy(leftChromosome, rigthChromosome, false));
            }
        }
        if (result.popuation.size == this.numberOfTournamentRounds) {

        } else {
            if (this.logDebug)
                console.log("Duplicate in population deleted, should be: " + this.numberOfTournamentRounds + ", but was: " + result.popuation.size + ".");
            result = this.fixPopulation(result, this.numberOfTournamentRounds);
        }
        if (this.logDebug)
            console.log("--------------- Trunament ended.");

        return Array.from(result.popuation.values());
    }

    private fixPopulation(populationModel: PopulationModel, populationSize: number): PopulationModel {
        populationModel = this.populationService.generatePopulationOrAddMissingIfPopulationSize(populationModel, populationSize);
        if (this.logDebug)
        console.log("Population size after fix: " + populationModel.popuation.size);
        return populationModel;
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

    private getLeftAndRigthChromomosomeByNumber(left: number, rigth: number, populationModel: PopulationModel) {
        var leftChromosome = populationModel.getChromosomeByIndex(left);
        var rigthChromosome = populationModel.getChromosomeByIndex(rigth);
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
            leftResult = leftChromosomeModel.sumOfF1;
            rightResult = rigthChromosomeModel.sumOfF1;
            //else by F2 factor
        } else {
            leftResult = leftChromosomeModel.sumOfF2;
            rightResult = rigthChromosomeModel.sumOfF2;
        }
        return leftResult < rightResult ? leftChromosomeModel : rigthChromosomeModel;
    }

    private shuffle(array) {
        for (let i = array.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [array[i], array[j]] = [array[j], array[i]];
        }
        return array;
    }

}
