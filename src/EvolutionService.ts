import { Matrix } from './Matrix';
import { EvolutionModel } from './EvolutionModel';

export interface IEvolutionService {
}

export class EvolutionService implements IEvolutionService {
    private maxDiffBeetweanEdges: number = 6;
    
    constructor(private _matrix: Matrix) {

    }

    public CreateEvolutionModelFromMatrix(matrix: Matrix): EvolutionModel {
        var result = new EvolutionModel();

        return result;
    }
}
