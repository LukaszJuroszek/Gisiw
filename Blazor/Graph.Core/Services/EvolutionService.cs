using Graph.Core.Models;

namespace Graph.Core.Services
{
    public interface IEvolutionService
    {
        void RunIteration(IPopulation population, IMatrix matrix);
    }
    public class EvolutionService : IEvolutionService
    {

        public void RunIteration(IPopulation population, IMatrix matrix)
        {
        }
    }
}
