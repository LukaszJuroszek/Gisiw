using Graph.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IEvolutionService
    {
        IPopulation RunIteration(IPopulation population, IMatrix matrix, double probability, int maxDiffBetweenNode);
        IEnumerable<IChromosome> GetBestChromosomes(ChromosomePart chromosomePart, IPopulation population, int numberOfTournamentRounds);

    }
    public class EvolutionService : IEvolutionService
    {
        public IPopulation RunIteration(IPopulation population, IMatrix matrix, double probability, int maxDiffBetweenNode)
        {
            var numberOfTournamentRounds = population.Members.Count();

            var bestChromosomesFromFirstPartByEdgeCountFactor = GetBestChromosomes(ChromosomePart.First, population, numberOfTournamentRounds);

            var bestChromosomesFromSecondPartByConnectedEdgeWeigthCount = GetBestChromosomes(ChromosomePart.Second, population, numberOfTournamentRounds);

            if (bestChromosomesFromFirstPartByEdgeCountFactor.Count() != numberOfTournamentRounds &&
                bestChromosomesFromSecondPartByConnectedEdgeWeigthCount.Count() != numberOfTournamentRounds)
            {
                throw new InvalidOperationException("Invalid best chromosome member count after turnament part");
            }

            return null;
        }

        public IEnumerable<IChromosome> GetBestChromosomes(ChromosomePart chromosomePart, IPopulation population, int numberOfTournamentRounds)
        {
            var halfOfElementsCount = (int)Math.Floor(numberOfTournamentRounds / 2.0d);

            var result = new List<IChromosome>();
            var guidMap = population.Members.Select(x => x.Id).ToArray();
            switch (chromosomePart)
            {
                case ChromosomePart.First:
                    for (var i = 0; i < numberOfTournamentRounds; i++)
                    {
                        var (left, rigth) = GenerateTwoRandomNumbers(0, halfOfElementsCount);
                        var leftChromosome = population.Members.FirstOrDefault(x => x.Id == guidMap[left]);
                        var rigthChromosome = population.Members.FirstOrDefault(x => x.Id == guidMap[rigth]);

                        IChromosome bestOne = GetBestChromosomeBy(leftChromosome, rigthChromosome, keyComparer: ChromosomeFactor.EdgeCount);

                        result.Add(bestOne);
                    }
                    break;
                case ChromosomePart.Second:
                    for (var i = 0; i < numberOfTournamentRounds; i++)
                    {
                        var (left, rigth) = GenerateTwoRandomNumbers(halfOfElementsCount, population.Members.Count());
                        var leftChromosome = population.Members.FirstOrDefault(x => x.Id == guidMap[left]);
                        var rigthChromosome = population.Members.FirstOrDefault(x => x.Id == guidMap[rigth]);

                        IChromosome bestOne = GetBestChromosomeBy(leftChromosome, rigthChromosome, keyComparer: ChromosomeFactor.ConnectedEdgeWeigthSum);

                        result.Add(bestOne);
                    }
                    break;
                case ChromosomePart.Unknown:
                default:
                    break;
            }

            return result;
        }

        private static IChromosome GetBestChromosomeBy(IChromosome leftChromosome, IChromosome rigthChromosome, ChromosomeFactor keyComparer)
        {
            return leftChromosome.Factors[keyComparer] > rigthChromosome.Factors[keyComparer] ? leftChromosome : rigthChromosome;
        }

        private (int left, int rigth) GenerateTwoRandomNumbers(int minValue, int maxValue)
        {
            var random = new Random();
            var left = -2;
            var rigth = -1;
            do
            {
                left = random.Next(minValue, maxValue);
                rigth = random.Next(minValue, maxValue);
            } while (left == rigth);

            return (left, rigth);
        }
    }
}
