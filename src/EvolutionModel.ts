export interface IEvolutionModel {
    sumOfF1: number;
    sumOfF2: number;
    chromosome: Array<ChromosomeElement>;
}
export class EvolutionModel implements IEvolutionModel {
    public sumOfF1: number;
    public sumOfF2: number;
    public chromosome: Array<ChromosomeElement>;

    constructor() {
        this.chromosome = new Array<ChromosomeElement>();
    }
}

export interface IChromosomeElement {
    nodeNumber: number;
    isFirstPart: boolean;
}

export class ChromosomeElement implements IChromosomeElement {
    nodeNumber: number;
    isFirstPart: boolean;
    constructor(nodeNumber: number, isFirstPart: boolean) {
        this.nodeNumber = nodeNumber;
        this.isFirstPart = isFirstPart
    }
}