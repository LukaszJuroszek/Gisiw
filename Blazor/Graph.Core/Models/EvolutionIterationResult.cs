
using System.Linq;

namespace Graph.Core.Models
{
    public interface IPopulationResult
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
        public IChromosome BestChromosome => Population.Members.OrderByDescending(x => x.FactorsSum).FirstOrDefault();

        public EvolutionIterationResult(IPopulation population, int iteration)
        {
            Population = population;
            Iteration = iteration;
        }
    }

    public class InitializedPopulationResult : IInitializedPopulationResult
    {
        public IPopulation Population { get; }
        public int Iteration => 0;
        public IChromosome BestChromosome => Population.Members.OrderByDescending(x => x.FactorsSum).FirstOrDefault();

        public InitializedPopulationResult(IPopulation population)
        {
            Population = population;
        }
    }
}
