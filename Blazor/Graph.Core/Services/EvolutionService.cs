using Graph.Core.Models;
using Graph.Core.Utils;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IEvolutionService
    {
        IChromosome[] GetBestChromosomes(ChromosomePart chromosomePart, IPopulation population,
            int numberOfTournamentRounds);
        IChromosome[] MutateChromosomeByNodeFlipping(IChromosome[] chromosomesByEdgeCount,
            IChromosome[] chromosomesByConnectedEdge, IMatrix matrix, int maxDiffBetweenNode);
        IEvolutionIterationResult RunIteration(IPopulationResult population, IMatrix matrix
            , int maxDiffBetweenNode);
        IEvolutionIterationResult[] RunIterations(int iterations, int maxDiffBetweenNode,
            IPopulationResult population, IMatrix matrix);
    }

    public class EvolutionService : IEvolutionService
    {
        private readonly MiniProfiler _profiler;
        private readonly IChromosomeService _chromosomeService;

        public EvolutionService(IChromosomeService chromosomeService)
        {
            _profiler = MiniProfiler.StartNew(nameof(EvolutionService));
            _chromosomeService = chromosomeService;
        }

        public IEvolutionIterationResult[] RunIterations(int iterations, int maxDiffBetweenNode, IPopulationResult population, IMatrix matrix)
        {
            var result = new IEvolutionIterationResult[iterations];

            result[0] = RunIteration(population, matrix, maxDiffBetweenNode);

            for (var i = 1; i < iterations; i++)
            {
                result[i] = RunIteration(result[i - 1], matrix, maxDiffBetweenNode);
            }

            return result;
        }

        public IEvolutionIterationResult RunIteration(IPopulationResult populationResult, IMatrix matrix, int maxDiffBetweenNode)
        {
            var halfOfElementsCount = (int)Math.Floor(populationResult.Population.Members.Count() / 2.0d);

            var bestChromosomesByEdgeCount = GetBestChromosomes(ChromosomePart.First, populationResult.Population, halfOfElementsCount);
            var bestChromosomesByConnectedEdge = GetBestChromosomes(ChromosomePart.Second, populationResult.Population, halfOfElementsCount);

            if (bestChromosomesByEdgeCount.Count() != halfOfElementsCount &&
                bestChromosomesByConnectedEdge.Count() != halfOfElementsCount)
            {
                throw new InvalidOperationException("Invalid best chromosome member count after turnament part");
            }

            var mutatedChromosomes = MutateChromosomeByNodeFlipping(bestChromosomesByEdgeCount, bestChromosomesByConnectedEdge, matrix, maxDiffBetweenNode);

            var members = mutatedChromosomes.OrderBy(x => Guid.NewGuid()).ToArray();

            return new EvolutionIterationResult(new Population(members), populationResult.Iteration + 1);
        }

        public IChromosome[] MutateChromosomeByNodeFlipping(IChromosome[] chromosomesByEdgeCount, IChromosome[] chromosomesByConnectedEdge, IMatrix matrix, int maxDiffBetweenNode)
        {
            var numberOfTimes = (int)chromosomesByEdgeCount.Length / 3.0;
            var random = new Random();
            using (_profiler.Step(nameof(MutateChromosomeByNodeFlipping)))
            {
                for (var i = 0; i < numberOfTimes; i++)
                {
                    var (left, rigth) = RandomNumberGeneratorUtils.GenerateTwoRandomNumbers(random, 0, chromosomesByEdgeCount.Length);

                    chromosomesByEdgeCount[left] = _chromosomeService.FlipNodeOnChromosoe(chromosomesByEdgeCount[left], maxDiffBetweenNode, matrix);
                    chromosomesByConnectedEdge[rigth] = _chromosomeService.FlipNodeOnChromosoe(chromosomesByConnectedEdge[rigth], maxDiffBetweenNode, matrix);
                }
            }

            var result = new List<IChromosome>();

            foreach (var chromosome in chromosomesByEdgeCount)
            {
                result.Add(chromosome.DeepCopy());
            }
            foreach (var chromosome in chromosomesByConnectedEdge)
            {
                result.Add(chromosome.DeepCopy());
            }

            return result.ToArray();
        }

        public IChromosome[] GetBestChromosomes(ChromosomePart chromosomePart, IPopulation population, int numberOfTournamentRounds)
        {
            var result = new IChromosome[numberOfTournamentRounds];
            var random = new Random();
            using (_profiler.Step($"{nameof(GetBestChromosomes)}{chromosomePart}"))
            {
                switch (chromosomePart)
                {
                    case ChromosomePart.First:
                        for (var i = 0; i < numberOfTournamentRounds; i++)
                        {
                            var (left, rigth) = RandomNumberGeneratorUtils.GenerateTwoRandomNumbers(random, 0, numberOfTournamentRounds);
                            var leftChromosome = population.Members.FirstOrDefault(x => x.Id == population.GuidMap[left]);
                            var rigthChromosome = population.Members.FirstOrDefault(x => x.Id == population.GuidMap[rigth]);

                            result[i] = GetBestChromosomeBy(leftChromosome, rigthChromosome, keyComparer: ChromosomeFactor.EdgeCount).DeepCopy();
                        }
                        break;
                    case ChromosomePart.Second:
                        for (var i = 0; i < numberOfTournamentRounds; i++)
                        {
                            var (left, rigth) = RandomNumberGeneratorUtils.GenerateTwoRandomNumbers(random, numberOfTournamentRounds, population.Members.Count());
                            var leftChromosome = population.Members.FirstOrDefault(x => x.Id == population.GuidMap[left]);
                            var rigthChromosome = population.Members.FirstOrDefault(x => x.Id == population.GuidMap[rigth]);

                            result[i] = GetBestChromosomeBy(leftChromosome, rigthChromosome, keyComparer: ChromosomeFactor.ConnectedEdgeWeigthSum).DeepCopy();
                        }
                        break;
                    case ChromosomePart.Unknown:
                    default:
                        break;
                }
            }
            return result;
        }

        private IChromosome GetBestChromosomeBy(IChromosome leftChromosome, IChromosome rigthChromosome, ChromosomeFactor keyComparer)
        {
            return leftChromosome.Factors[keyComparer] > rigthChromosome.Factors[keyComparer] ? leftChromosome : rigthChromosome;
        }
    }
}
