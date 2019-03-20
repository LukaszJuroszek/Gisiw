export class EvolutionModel {
    public sumOfF1: number;
    public sumOfF2: number;
    public chromosome: Array<ChromosomeElement>;

    constructor() {
        this.chromosome = new Array<ChromosomeElement>();
    }
}

export class ChromosomeElement {
    nodeNumber: number;
    isFirstPart: boolean;
    constructor(nodeNumber: number, isFirstPart: boolean) {
        this.nodeNumber = nodeNumber;
        this.isFirstPart = isFirstPart
    }
}