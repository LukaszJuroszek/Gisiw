export class ChromosomeModel {
    public sumOfF1: number;
    public sumOfF2: number;
    public chromosome: Array<ChromosomeElement>;

    constructor() {
        this.chromosome = new Array<ChromosomeElement>();
    }

    public getString(): string {
        var result: Array<String> = new Array<String>()
        for (let i = 0; i < this.chromosome.length; i++) {
            var node = this.chromosome[i].isFirstPart ? "0" : "1";
            result.push(node);
        }
        return result.toString();
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