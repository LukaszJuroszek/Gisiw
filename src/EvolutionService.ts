import { Matrix } from './Matrix';
export interface IEvolutionService {
}

export class EvolutionService implements IEvolutionService {
    constructor(private _matrix: Matrix) {

    }
    
}