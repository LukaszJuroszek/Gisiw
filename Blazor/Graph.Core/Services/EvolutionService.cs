using Graph.Core.Models;
using Graph.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IEvolutionService
    {
        IChromosome[] GetBestChromosomes(ChromosomePart chromosomePart, IPopulation population, int numberOfTournamentRounds);
        IChromosome[] MutateChromosomeByNodeFlipping(IChromosome[] chromosomesByEdgeCount, IChromosome[] chromosomesByConnectedEdge, IMatrix matrix, int maxDiffBetweenNode);
        IEvolutionIterationResult RunIteration(IPopulationResult population, IMatrix matrix, int maxDiffBetweenNode);
    }

    public class EvolutionService : IEvolutionService
    {
        private readonly IChromosomeService _chromosomeService;

        public EvolutionService(IChromosomeService chromosomeService)
        {
            _chromosomeService = chromosomeService;
        }

        public IEvolutionIterationResult RunIteration(IPopulationResult populationResult, IMatrix matrix, int maxDiffBetweenNode)
        {
            var numberOfTournamentRounds = populationResult.Population.Members.Count();

            var bestChromosomesByEdgeCount = GetBestChromosomes(ChromosomePart.First, populationResult.Population, numberOfTournamentRounds);

            var bestChromosomesByConnectedEdge = GetBestChromosomes(ChromosomePart.Second, populationResult.Population, numberOfTournamentRounds);

            if (bestChromosomesByEdgeCount.Count() != numberOfTournamentRounds &&
                bestChromosomesByConnectedEdge.Count() != numberOfTournamentRounds)
            {
                throw new InvalidOperationException("Invalid best chromosome member count after turnament part");
            }

            var mutatedChromosomes = MutateChromosomeByNodeFlipping(bestChromosomesByEdgeCount, bestChromosomesByConnectedEdge, matrix, maxDiffBetweenNode);

            var members = mutatedChromosomes.OrderBy(x => new Random().Next()).ToArray();

            return new EvolutionIterationResult(new Population(members), (populationResult.Iteration + 1));
        }

        public IChromosome[] MutateChromosomeByNodeFlipping(IChromosome[] chromosomesByEdgeCount, IChromosome[] chromosomesByConnectedEdge, IMatrix matrix, int maxDiffBetweenNode)
        {
            var numberOfTimes = 10;

            for (var i = 0; i < numberOfTimes; i++)
            {
                var (left, rigth) = RandomNumberGeneratorUtils.GenerateTwoRandomNumbers(0, chromosomesByEdgeCount.Length);

                chromosomesByEdgeCount[left] = _chromosomeService.FlipNodeOnChromosoe(chromosomesByEdgeCount[left], maxDiffBetweenNode, matrix);
                chromosomesByConnectedEdge[rigth] = _chromosomeService.FlipNodeOnChromosoe(chromosomesByConnectedEdge[rigth], maxDiffBetweenNode, matrix);
            }

            var result = new List<IChromosome>();
            result.AddRange(chromosomesByEdgeCount);
            result.AddRange(chromosomesByConnectedEdge);

            return result.ToArray();
        }

        public IChromosome[] GetBestChromosomes(ChromosomePart chromosomePart, IPopulation population, int numberOfTournamentRounds)
        {
            var halfOfElementsCount = (int)Math.Floor(numberOfTournamentRounds / 2.0d);

            var result = new IChromosome[halfOfElementsCount];
            switch (chromosomePart)
            {
                case ChromosomePart.First:
                    for (var i = 0; i < numberOfTournamentRounds; i++)
                    {
                        var (left, rigth) = RandomNumberGeneratorUtils.GenerateTwoRandomNumbers(0, halfOfElementsCount);
                        var leftChromosome = population.Members.FirstOrDefault(x => x.Id == population.GuidMap[left]);
                        var rigthChromosome = population.Members.FirstOrDefault(x => x.Id == population.GuidMap[rigth]);

                        IChromosome bestOne = GetBestChromosomeBy(leftChromosome, rigthChromosome, keyComparer: ChromosomeFactor.EdgeCount);

                        result[i] = bestOne;
                    }
                    break;
                case ChromosomePart.Second:
                    for (var i = 0; i < numberOfTournamentRounds; i++)
                    {
                        var (left, rigth) = RandomNumberGeneratorUtils.GenerateTwoRandomNumbers(halfOfElementsCount, population.Members.Count());
                        var leftChromosome = population.Members.FirstOrDefault(x => x.Id == population.GuidMap[left]);
                        var rigthChromosome = population.Members.FirstOrDefault(x => x.Id == population.GuidMap[rigth]);

                        IChromosome bestOne = GetBestChromosomeBy(leftChromosome, rigthChromosome, keyComparer: ChromosomeFactor.ConnectedEdgeWeigthSum);

                        result[i] = bestOne;
                    }
                    break;
                case ChromosomePart.Unknown:
                default:
                    break;
            }

            return result;
        }

        private IChromosome GetBestChromosomeBy(IChromosome leftChromosome, IChromosome rigthChromosome, ChromosomeFactor keyComparer)
        {
            return leftChromosome.Factors[keyComparer] > rigthChromosome.Factors[keyComparer] ? leftChromosome : rigthChromosome;
        }
    }
}
