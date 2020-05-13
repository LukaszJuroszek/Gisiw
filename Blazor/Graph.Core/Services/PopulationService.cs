using Graph.Core.Models;
using Graph.Core.Utils;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IPopulationService
    {
        IInitializedPopulationResult Initialize(IMatrix matrix, int populationSize, int maxDiffBetweenNode);
        IChromosome GenerateChromosome(IMatrix matrix, int maxDiffBetweenNode);
    }
    public class PopulationService : IPopulationService
    {
        private readonly MiniProfiler _profiler;
        private double _probability = 0.5d;
        private readonly IChromosomeService _chromosomeService;

        public PopulationService(IChromosomeService chromosomeService)
        {
            _profiler = MiniProfiler.StartNew(nameof(PopulationService));
            _chromosomeService = chromosomeService;
        }

        public IInitializedPopulationResult Initialize(IMatrix matrix, int populationSize, int maxDiffBetweenNode)
        {
            var result = new HashSet<IChromosome>();
            using (_profiler.Step(nameof(Initialize)))
            {
                if (result.Count < populationSize)
                {
                    do
                    {
                        result.Add(GenerateChromosome(matrix, maxDiffBetweenNode));
                    }
                    while (result.Count < populationSize);
                }
            }
            //_profiler.Stop();
            //Console.WriteLine(_profiler.RenderPlainText());
            return new InitializedPopulationResult(new Population(result));
        }

        public IChromosome GenerateChromosome(IMatrix matrix, int maxDiffBetweenNode)
        {
            var tryCount = 0;
            var distribution = new Dictionary<int, ChromosomePart>();
            var random = new Random();
            do
            {
                _probability = ProbabilityUtils.AdaptProbability(_probability, tryCount);
                distribution.Clear();

                for (var i = 0; i < matrix.Elements.Length; i++)
                {
                    distribution.Add(i, random.NextDouble() < _probability ? ChromosomePart.First : ChromosomePart.Second);
                }

                tryCount++;

            } while (_chromosomeService.IsNodeCountValid(distribution, maxDiffBetweenNode) == false);

            return new Chromosome
            {
                Id = Guid.NewGuid(),
                Distribution = distribution,
                Factors = _chromosomeService.GetChromosomeFactors(distribution, matrix)
            };
        }
    }
}
