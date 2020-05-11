
namespace Graph.Core.Models
{
    public interface IPopulationResult
    {
        IPopulation Population { get; }
        int Iteration { get; }
    }

    public interface IEvolutionIterationResult: IPopulationResult
    {

    }

    public interface IInitializedPopulationResult : IPopulationResult
    {

    }

    public class EvolutionIterationResult : IEvolutionIterationResult
    {
        public IPopulation Population { get; }
        public int Iteration { get; }

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

        public InitializedPopulationResult(IPopulation population)
        {
            Population = population;
        }
    }
}
