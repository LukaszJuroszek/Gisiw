using Graph.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IPopulationService
    {
        IEnumerable<ChromosomeModel> Initialize(MatrixModel matrix, int populationSize, double probability, int maxDiffBetweenNode);
        ChromosomeModel GenerateChromosome(MatrixModel matrix, double probability, int maxDiffBetweenNode);
    }
    public class PopulationService : IPopulationService
    {
        private readonly IChromosomeService _chromosomeService;

        public PopulationService(IChromosomeService chromosomeService)
        {
            _chromosomeService = chromosomeService;
        }

        public IEnumerable<ChromosomeModel> Initialize(MatrixModel matrix, int populationSize, double probability, int maxDiffBetweenNode)
        {
            var result = new HashSet<ChromosomeModel>();
            if (result.Count < populationSize)
            {
                do
                {
                    result.Add(GenerateChromosome(matrix, probability, maxDiffBetweenNode));
                }
                while (result.Count < populationSize);
            }
            return result;
        }

        public ChromosomeModel GenerateChromosome(MatrixModel matrix, double probability, int maxDiffBetweenNode)
        {
            ChromosomeModel result;
            var random = new Random();
            do
            {
                result = new ChromosomeModel
                {
                    Distribution = new Dictionary<int, ChromosomePart>()
                };
                for (var i = 0; i < matrix.Elements.Length; i++)
                {
                    result.Distribution.Add(i, random.NextDouble() < probability ? ChromosomePart.First : ChromosomePart.Second);
                }
                var (edgeCount, edgeWeigthCount) = _chromosomeService.GetConnectedEdgeCountAndWegithCount(result, matrix);
                result.FactorSum1 = edgeCount;
                result.FactorSum2 = edgeWeigthCount;

            } while (_chromosomeService.IsNodeCountValid(result, maxDiffBetweenNode) == false);

            return result;
        }
    }
}
