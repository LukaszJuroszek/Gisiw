using Graph.Core.Models;
using Graph.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IPopulationService
    {
        IPopulation Initialize(IMatrix matrix, int populationSize, int maxDiffBetweenNode);
        IChromosome GenerateChromosome(IMatrix matrix, int maxDiffBetweenNode);
        IChromosome GetBestChromosome(IPopulation population);
    }
    public class PopulationService : IPopulationService
    {
        private double _probability = 0.5d;
        private readonly IChromosomeService _chromosomeService;

        public PopulationService(IChromosomeService chromosomeService)
        {
            _chromosomeService = chromosomeService;
        }

        public IPopulation Initialize(IMatrix matrix, int populationSize, int maxDiffBetweenNode)
        {
            var result = new HashSet<IChromosome>();
            if (result.Count < populationSize)
            {
                do
                {
                    result.Add(GenerateChromosome(matrix, maxDiffBetweenNode));
                }
                while (result.Count < populationSize);
            }
          
            return new Population { Members = result };
        }

        public IChromosome GenerateChromosome(IMatrix matrix, int maxDiffBetweenNode)
        {
            var tryCount = 0;
            Chromosome result;
            var random = new Random();
            do
            {
                _probability = ProbabilityUtils.AdaptProbability(_probability, tryCount);

                result = new Chromosome
                {
                    Id = Guid.NewGuid(),
                    Distribution = new Dictionary<int, ChromosomePart>()
                };

                for (var i = 0; i < matrix.Elements.Length; i++)
                {
                    result.Distribution.Add(i, random.NextDouble() < _probability ? ChromosomePart.First : ChromosomePart.Second);
                }

                tryCount++;

            } while (_chromosomeService.IsNodeCountValid(result, maxDiffBetweenNode) == false);

            var (edgeCount, edgeWeigthCount) = _chromosomeService.GetConnectedEdgeCountAndWegithCount(result, matrix);
            result.FactorSum1 = edgeCount;
            result.FactorSum2 = edgeWeigthCount;

            return result;
        }

        public IChromosome GetBestChromosome(IPopulation population)
        {
            return population.Members.Aggregate((current, next) => current.FactorSums > next.FactorSums ? current : next);
        }
    }
}
