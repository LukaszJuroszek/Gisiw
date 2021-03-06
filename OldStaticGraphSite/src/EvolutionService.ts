import { ChromosomeModel } from './ChromosomeModel';
import { PopulationService } from './populationService';
import { Matrix } from './Matrix';

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
    public runIteration(population: Array<ChromosomeModel>, probability: number, matrix: Matrix, maxDiffBetweenNode: number): Array<ChromosomeModel> {
        var bestCollectionByF1: Array<ChromosomeModel> = this.getBestChromosomeModelsBy(true, population, probability, matrix, maxDiffBetweenNode);
        var bestCollectionByF2: Array<ChromosomeModel> = this.getBestChromosomeModelsBy(false, population, probability, matrix, maxDiffBetweenNode);

        if (bestCollectionByF1.length != population.length / 2)
            console.log("bestCollectionByF1.length is INVALID " + bestCollectionByF1.length);

        if (bestCollectionByF2.length != population.length / 2)
            console.log("bestCollectionByF2.length is INVALID " + bestCollectionByF2.length);

        this.mutateChromosomes(bestCollectionByF1, bestCollectionByF2, maxDiffBetweenNode, false, true);

        var temp = this.shufle(bestCollectionByF1.concat(bestCollectionByF2));

        population = temp;

        return population;
    }

    private mutateChromosomes(bestCollectionByF1: ChromosomeModel[],
        bestCollectionByF2: ChromosomeModel[],
        maxDiffBetweenNode: number,
        oneNodeMutate: boolean,
        flipNodeMutate: boolean) {
        var numberOfThimes: number = 10// this.generateNumbers(bestCollectionByF1.length / 20);

        for (let i = 0; i < numberOfThimes; i++) {
            var rNumber = this.generateNumbers(bestCollectionByF1.length);
            var lNumber = this.generateNumbers(bestCollectionByF2.length);


            if (oneNodeMutate) {
                if (this.canChangeChromosomeByOneNode(bestCollectionByF1[rNumber], maxDiffBetweenNode)) {
                    bestCollectionByF1[rNumber] = this.mutateChromosomeByOneNode(bestCollectionByF1[rNumber], maxDiffBetweenNode);
                }

                if (this.canChangeChromosomeByOneNode(bestCollectionByF2[lNumber], maxDiffBetweenNode)) {
                    bestCollectionByF2[lNumber] = this.mutateChromosomeByOneNode(bestCollectionByF2[lNumber], maxDiffBetweenNode);
                }
            }

            if (flipNodeMutate) {
                bestCollectionByF1[rNumber] = this.mutateChromosomeByFippingNode(bestCollectionByF1[rNumber], maxDiffBetweenNode);
                bestCollectionByF2[lNumber] = this.mutateChromosomeByFippingNode(bestCollectionByF2[lNumber], maxDiffBetweenNode);
            }

            if (!oneNodeMutate && !flipNodeMutate) {
                console.log("Wrong mutateChromosomes arguments!");
            }
        }
    }

    private mutateChromosomeByOneNode(chromosomeModel: ChromosomeModel, maxDiffBetweenNode: number): ChromosomeModel {
        var temp: ChromosomeModel = chromosomeModel;
        var randomNodeNumber: number = 0;
        do {
            do {
                randomNodeNumber = this.generateNumbers(chromosomeModel.chromosome.length);
            } while (chromosomeModel.chromosome[randomNodeNumber].chromosomePartNumber != 0)

            chromosomeModel.chromosome[randomNodeNumber].chromosomePartNumber = 1;
            chromosomeModel = this.populationService.setSumOfF1AndF2(chromosomeModel);

            if (!this.populationService.isNodeCountValid(chromosomeModel, maxDiffBetweenNode)) {
                chromosomeModel = temp.getCopy()
            } else {
                break;
            }
        } while (!this.populationService.isNodeCountValid(chromosomeModel, maxDiffBetweenNode))

        return this.populationService.setSumOfF1AndF2(chromosomeModel).getCopy();
    }

    private canChangeChromosomeByOneNode(chromosomeModel: ChromosomeModel, maxDiffBetweenNode: number): boolean {
        var result: boolean = false;
        var [firstPartSum, secondPartSum] = this.populationService.getNodeChromosomePartCount(chromosomeModel);

        if (Math.abs(firstPartSum - secondPartSum) < maxDiffBetweenNode)
            result = true;

        return result;
    }

    private mutateChromosomeByFippingNode(chromosomeModel: ChromosomeModel, maxDiffBetweenNode: number): ChromosomeModel {
        var temp: ChromosomeModel = chromosomeModel;
        var firstRandomNodeNumber: number = 0;
        var secondRandomNodeNumber: number = 0;
        var maxIteration: number = 20;
        var currentIteration: number = 0;
        do {
            do {
                do {
                    firstRandomNodeNumber = this.generateNumbers(chromosomeModel.chromosome.length);
                    secondRandomNodeNumber = this.generateNumbers(chromosomeModel.chromosome.length);
                } while ((firstRandomNodeNumber != secondRandomNodeNumber) == false)

            } while (this.areChromosomePartsNotEqualAnd(
                chromosomeModel,
                firstRandomNodeNumber,
                secondRandomNodeNumber) == false)

            chromosomeModel.chromosome[firstRandomNodeNumber].chromosomePartNumber = 1;
            chromosomeModel.chromosome[secondRandomNodeNumber].chromosomePartNumber = 0;

            chromosomeModel = this.populationService.setSumOfF1AndF2(chromosomeModel);

            currentIteration += 1;

            if (!this.populationService.isNodeCountValid(chromosomeModel, maxDiffBetweenNode)) {
                chromosomeModel = temp.getCopy()
            } else {
                break;
            }

            if (currentIteration > maxIteration) {
                console.log("Max iteration occurd ! error");
                chromosomeModel = temp.getCopy()
                break;
            }
        } while (!this.populationService.isNodeCountValid(chromosomeModel, maxDiffBetweenNode))

        return this.populationService.setSumOfF1AndF2(chromosomeModel).getCopy();
    }

    private areChromosomePartsNotEqualAnd(chromosomeModel: ChromosomeModel, firstRandomNodeNumber: number, secondRandomNodeNumber: number) {
        var firstChromosomePartNumber = chromosomeModel.chromosome[firstRandomNodeNumber].chromosomePartNumber;
        var secondChromosomePartNumber = chromosomeModel.chromosome[secondRandomNodeNumber].chromosomePartNumber;

        if (firstRandomNodeNumber == secondChromosomePartNumber) {
            return false;
        }

        return firstChromosomePartNumber == 0 && secondChromosomePartNumber == 1
    }

    private getBestChromosomeModelsBy(by: boolean, population: Array<ChromosomeModel>, probability: number, matrix: Matrix, maxDiffBetweenNode: number): Array<ChromosomeModel> {
        var halfOfElementsCount: number = Math.floor(population.length / 2.0);
        var result: Array<ChromosomeModel> = new Array<ChromosomeModel>()
        if (by) { //if by F1 factor
            for (let i = 0; i < this.numberOfTournamentRounds; i++) {
                var { left, rigth } = this.generateTwoNumbers(halfOfElementsCount);
                var { leftChromosome, rigthChromosome } = this.getLeftAndRigthChromomosomeByNumber(left, rigth, population);

                result.push(this.selectBestBy(leftChromosome, rigthChromosome, true));
            }

        } else {   //else by F2 factor
            for (let i = 0; i < this.numberOfTournamentRounds; i++) {
                var { left, rigth } = this.generateTwoNumbers(halfOfElementsCount, halfOfElementsCount);
                var { leftChromosome, rigthChromosome } = this.getLeftAndRigthChromomosomeByNumber(left, rigth, population);

                result.push(this.selectBestBy(leftChromosome, rigthChromosome, false));
            }
        }

        if (result.length < this.numberOfTournamentRounds) {
            var missingChromosomesCount = this.numberOfTournamentRounds - result.length
            if (missingChromosomesCount < 0)
                console.log("missingChromosomesCount is INVALID ! " + missingChromosomesCount)
            var missingChromosomes = this.populationService.generateChromosomes(missingChromosomesCount, matrix, probability, maxDiffBetweenNode);

            missingChromosomes.forEach(element => {
                result.push(element);
            });

            return result;
        } else {
            return result;
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
        return Math.floor((Math.random() * to));
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

    private shufle(array: Array<ChromosomeModel>): Array<ChromosomeModel> {
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
