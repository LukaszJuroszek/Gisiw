using System.Linq;

namespace Graph.Core.Models
{
    public interface IPopulationResult : IDeepCopy<IPopulationResult>
    {
        IPopulation Population { get; }
        IChromosome BestChromosome { get; }
        int Iteration { get; }
    }

    public interface IEvolutionIterationResult : IPopulationResult
    {

    }

    public interface IInitializedPopulationResult : IPopulationResult
    {

    }

    public class EvolutionIterationResult : IEvolutionIterationResult
    {
        public IPopulation Population { get; }
        public int Iteration { get; }
        public IChromosome BestChromosome => Population.Members.OrderBy(x => x.FactorsSum)
                                                               .FirstOrDefault()
                                                               .DeepCopy();

        public EvolutionIterationResult(IPopulation population, int iteration)
        {
            Population = population;
            Iteration = iteration;
        }

        public IPopulationResult DeepCopy() => new EvolutionIterationResult(Population.DeepCopy(), Iteration);
    }

    public class InitializedPopulationResult : IInitializedPopulationResult
    {
        public IPopulation Population { get; }
        public int Iteration => 0;
        public IChromosome BestChromosome => Population.Members.OrderBy(x => x.FactorsSum)
                                                               .FirstOrDefault()
                                                               .DeepCopy();

        public InitializedPopulationResult(IPopulation population)
        {
            Population = population;
        }

        public IPopulationResult DeepCopy() => new InitializedPopulationResult(Population.DeepCopy());
    }
}
