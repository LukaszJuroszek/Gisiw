using Graph.Core.Models;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IPopulationService
    {
        IEnumerable<ChromosomeModel> Initialize(int populationSize, MatrixModel matrix, double probability, int maxDiffBetweenNode);
        ChromosomeModel GenerateChromosome(MatrixModel matrix, double probabilit, int maxDiffBetweenNode);
    }
    public class PopulationService : IPopulationService
    {
        public ChromosomeModel GenerateChromosome(MatrixModel matrix, double probabilit, int maxDiffBetweenNode)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ChromosomeModel> Initialize(int populationSize, MatrixModel matrix, double probability, int maxDiffBetweenNode)
        {
            throw new System.NotImplementedException();
        }
    }
}
