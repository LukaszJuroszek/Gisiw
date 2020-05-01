using Graph.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IPopulationService
    {
        IEnumerable<ChromosomeModel> Initialize(MatrixModel matrix, int populationSize, double probability, int maxDiffBetweenNode);
        ChromosomeModel GenerateChromosome(MatrixModel matrix, double startProbability, int maxDiffBetweenNode);
    }
    public class PopulationService : IPopulationService
    {
        private readonly IChromosomeService _chromosomeService;

        public PopulationService(IChromosomeService chromosomeService)
        {
            _chromosomeService = chromosomeService;
        }

        public IEnumerable<ChromosomeModel> Initialize(MatrixModel matrix, int populationSize, double startProbability, int maxDiffBetweenNode)
        {
            var result = new HashSet<ChromosomeModel>();
            if (result.Count < populationSize)
            {
                do
                {
                    result.Add(GenerateChromosome(matrix, startProbability, maxDiffBetweenNode));
                }
                while (result.Count < populationSize);
            }
            return result;
        }

        public ChromosomeModel GenerateChromosome(MatrixModel matrix, double startProbability, int maxDiffBetweenNode)
        {
            var tryCount = 0;
            ChromosomeModel result;
            var random = new Random();
            do
            {
                if (tryCount % 10 == 0 && tryCount != 0)
                {
                    if (startProbability < 0.5d && startProbability + 0.1d < 1d)
                    {
                        startProbability += 0.1d;
                    }
                    else if (startProbability > 0.5d && startProbability - 0.1d > 0d)
                    {
                        startProbability -= 0.1d;
                    }
                }

                result = new ChromosomeModel
                {
                    Distribution = new Dictionary<int, ChromosomePart>()
                };

                for (var i = 0; i < matrix.Elements.Length; i++)
                {
                    result.Distribution.Add(i, random.NextDouble() < startProbability ? ChromosomePart.First : ChromosomePart.Second);
                }

                tryCount++;
            } while (_chromosomeService.IsNodeCountValid(result, maxDiffBetweenNode) == false);

            var (edgeCount, edgeWeigthCount) = _chromosomeService.GetConnectedEdgeCountAndWegithCount(result, matrix);
            result.FactorSum1 = edgeCount;
            result.FactorSum2 = edgeWeigthCount;
            return result;
        }
    }
}
