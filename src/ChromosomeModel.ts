export class ChromosomeModel {
    public sumOfF1: number;
    public sumOfF2: number;
    public chromosome: Array<ChromosomeElement>;

    constructor() {
        this.chromosome = new Array<ChromosomeElement>();
    }
}

export class ChromosomeElement {
    nodeNumber: number;
    //chromosome is identifying spliting graph to two sub graph.
    isFirstPart: boolean;
    constructor(nodeNumber: number, isFirstPart: boolean) {
        this.nodeNumber = nodeNumber;
        this.isFirstPart = isFirstPart
    }
}