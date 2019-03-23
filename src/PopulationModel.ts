import { ChromosomeElement, ChromosomeModel } from "./ChromosomeModel";

export class PopulationModel {
    public popuation: Set<ChromosomeModel>;

    constructor(_popuation: Set<ChromosomeModel>) {
            this.popuation = _popuation
    }

    public getF1SumF2SumAndParetoPairs(): [number, number, Array<[number, number]>] {
        var sumf1: number = 0;
        var sumf2: number = 0;
        var pairs: Array<[number, number]> = new Array<[number, number]>();
        this.popuation.forEach(chromosomeModel => {
            sumf1 += chromosomeModel.sumOfF1;
            sumf2 += chromosomeModel.sumOfF2;
            pairs.push([chromosomeModel.sumOfF1, chromosomeModel.sumOfF2]);
        });
        return [sumf1, sumf2, pairs];
    }

    public getPopulationCount(): number {
        return this.popuation.size;
    }

    public getChromosomeByIndex(index: number): ChromosomeModel {
        return Array.from(this.popuation.values())[index];
    }
}