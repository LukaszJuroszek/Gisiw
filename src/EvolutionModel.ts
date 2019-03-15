export interface IEvolutionModel {
    sumOfF1: number;
    sumOfF2: number;
    chromosome: Array<ChromosomeElement>;
}
export class EvolutionModel implements IEvolutionModel {
    public sumOfF1: number;
    public sumOfF2: number;
    public chromosome: Array<ChromosomeElement>;
}

export interface IChromosomeElement {
    indexNumber: number;
    nodeNumber: number;

}
export class ChromosomeElement implements IChromosomeElement {
    indexNumber: number;
    nodeNumber: number;
}