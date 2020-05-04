export class ChromosomeModel {
    public iterationNumber: number;
    public sumOfF1: number;
    public sumOfF2: number;
    public chromosome: Array<ChromosomeElement>;

    constructor() {
        this.chromosome = new Array<ChromosomeElement>();
        this.sumOfF1 = 99999999;
        this.sumOfF2 = 99999999;
    }

    public getString(): string {
        var result: Array<String> = new Array<String>()
        for (let i = 0; i < this.chromosome.length; i++) {
            var node = this.chromosome[i].chromosomePartNumber.toString();
            result.push(node);
        }
        return result.toString();
    }

    public getStringWithSums(): string {
        var result: Array<String> = new Array<String>()
        for (let i = 0; i < this.chromosome.length; i++) {
            var node = this.chromosome[i].chromosomePartNumber.toString();
            result.push(node);
        }
        return result.toString() + "\t sumOfF1: " + this.sumOfF1 + " sumOfF2: " + this.sumOfF2 + "\t iteration: " + this.iterationNumber + "\n" +
            "sumOfF1AndF2: " + (this.sumOfF1 + this.sumOfF2);
    }

    public getSumOfF1AndF2(): number {
        return this.sumOfF1 + this.sumOfF2;
    }

    public getCopy(): ChromosomeModel {
        var copy = new ChromosomeModel();
        copy.iterationNumber = this.iterationNumber;
        copy.sumOfF1 = this.sumOfF1;
        copy.sumOfF2 = this.sumOfF2;
        for (let i = 0; i < this.chromosome.length; i++) {
            var temp: ChromosomeElement = new ChromosomeElement(
                this.chromosome[i].nodeNumber,
                this.chromosome[i].chromosomePartNumber);
                copy.chromosome.push(temp);
        }
        return copy;
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