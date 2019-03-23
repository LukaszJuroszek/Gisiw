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

    public getStringWithSums(): string {
        var result: Array<String> = new Array<String>()
        for (let i = 0; i < this.chromosome.length; i++) {
            var node = this.chromosome[i].isFirstPart ? "0" : "1";
            result.push(node);
        }
        return result.toString() + " sumOfF1: " + this.sumOfF1 + " sumOfF2: " + this.sumOfF2;
    }
}

export class ChromosomeElement {
    nodeNumber: number;
    //chromosome is identifying spliting graph to two sub graph.
    chromosomePartNumber: number;
    constructor(nodeNumber: number, chromosomePartNumber: number) {
        this.nodeNumber = nodeNumber;
        this.chromosomePartNumber = chromosomePartNumber
    }
}