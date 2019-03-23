import { ChromosomeModel } from './ChromosomeModel';
import { PopulationService } from './PopulationService';

export class EvolutionService {

    constructor(private numberOfTournamentRounds: number,
        private populationService: PopulationService) { }

    // i. Podzielić populacje na dwie podpopulacje
    // ii. Ocenić je według dwóch różnych wag
    // iii. Sukcesja turniejowa
    // iv. Dokonać przetasowania
    // v. Na kolejnej populacji (potomnej) przeprowadzać operacje genetyczne
    // vi. Wydzielić front Pareto dla każdej populacji
    // vii. Graficznie przedstawiać populację Pi oraz Pi+1 
    public runIteration(population: Array<ChromosomeModel>): Array<ChromosomeModel> {

        var bestCollectionByF1: Array<ChromosomeModel> = this.getBestChromosomeModelsBy(true, population);
        var bestCollectionByF2: Array<ChromosomeModel> = this.getBestChromosomeModelsBy(false, population);

        //copy new population to model
        let temp = this.shuffleshuffle(bestCollectionByF1.concat(bestCollectionByF2));
        // let temp = bestCollectionByF1.concat(bestCollectionByF2);
        population = temp;
        return population;
    }

    public getBestChromosomeModelsBy(by: boolean, population: Array<ChromosomeModel>): Array<ChromosomeModel> {
        var halfOfElementsCount: number = Math.floor(population.length / 2.0);
        var result: Array<ChromosomeModel> = new Array<ChromosomeModel>()
        if (by) { //if by F1 factor
            for (let i = 0; i < this.numberOfTournamentRounds; i++) {
                var { left, rigth } = this.generateTwoNumbers(halfOfElementsCount);
                var { leftChromosome, rigthChromosome } = this.getLeftAndRigthChromomosomeByNumber(left, rigth, population);
                // console.log("left: " + left + " rigth: " + rigth)

                result.push(this.selectBestBy(leftChromosome, rigthChromosome, true));
            }

        } else { //else by F2 factor
            for (let i = 0; i < this.numberOfTournamentRounds; i++) {
                var { left, rigth } = this.generateTwoNumbers(halfOfElementsCount, halfOfElementsCount);
                // console.log("left: " + left + " rigth: " + rigth)
                var { leftChromosome, rigthChromosome } = this.getLeftAndRigthChromomosomeByNumber(left, rigth, population);

                result.push(this.selectBestBy(leftChromosome, rigthChromosome, false));
            }
        }
        if (result.length < this.numberOfTournamentRounds) {
            result = this.populationService.generatePopulationOrAddMissingIfPopulationSize(result, this.numberOfTournamentRounds);
            return Array.from(result);
        } else {
            return Array.from(result);
        }
    }

    private generateTwoNumbers(to: number, plusValue: number = 0) {
        var left: number = -2;
        var rigth: number = -1;
        do {
            var left = this.generateNumbers(to);
            var rigth = this.generateNumbers(to);
        } while (left == rigth)
        left += plusValue
        rigth += plusValue
        return { left, rigth };
    }

    private getLeftAndRigthChromomosomeByNumber(left: number, rigth: number, population: Array<ChromosomeModel>) {
        var leftChromosome = population[left];
        var rigthChromosome = population[rigth];
        return { leftChromosome, rigthChromosome };
    }

    private generateNumbers(to: number) {
        return Math.floor((Math.random() * to) + 0);
    }

    private selectBestBy(leftChromosomeModel: ChromosomeModel, rigthChromosomeModel: ChromosomeModel, by: boolean): ChromosomeModel {
        //if by F1 factor
        if (by) {
            return leftChromosomeModel.sumOfF1 < rigthChromosomeModel.sumOfF1 ? leftChromosomeModel : rigthChromosomeModel;
            //else by F2 factor
        } else {
            return leftChromosomeModel.sumOfF2 < rigthChromosomeModel.sumOfF2 ? leftChromosomeModel : rigthChromosomeModel;
        }
    }

    shuffleshuffle(array: Array<ChromosomeModel>): Array<ChromosomeModel> {
        var counter = array.length;

        // While there are elements in the array
        while (counter > 0) {
            // Pick a random index
            let index = Math.floor(Math.random() * counter);

            // Decrease counter by 1
            counter--;

            // And swap the last element with it
            let temp = array[counter];
            array[counter] = array[index];
            array[index] = temp;
        }

        return array;
    }
}
