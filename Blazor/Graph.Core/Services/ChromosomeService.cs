using Graph.Core.Models;
using Graph.Core.Utils;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IChromosomeService
    {
        bool IsNodeCountValid(Dictionary<int, ChromosomePart> distribution, int maxDiffBetweenNode);
        Dictionary<ChromosomeFactor, int> GetChromosomeFactors(Dictionary<int, ChromosomePart> distribution,
            IMatrix matrix);
        IChromosome FlipNodeOnChromosoe(IChromosome chromosome, int maxDiffBetweenNode, IMatrix matrix);
        IChromosome GenerateChromosome(IMatrix matrix, int maxDiffBetweenNode);
    }

    public class ChromosomeService : IChromosomeService
    {
        private readonly MiniProfiler _profiler;
        private double _probability = 0.5d;

        public ChromosomeService()
        {
            _profiler = MiniProfiler.StartNew(nameof(MiniProfiler));
        }

        public bool IsNodeCountValid(Dictionary<int, ChromosomePart> distribution, int maxDiffBetweenNode)
        {
            var nodeSum = distribution.Count(x => x.Value == ChromosomePart.First);
            var firstPart = distribution.Count - nodeSum;
            var secondPart = distribution.Count - firstPart;
            return Math.Abs(firstPart - secondPart) < maxDiffBetweenNode;
        }

        public Dictionary<ChromosomeFactor, int> GetChromosomeFactors(Dictionary<int, ChromosomePart> distribution, IMatrix matrix)
        {
            var edgeCount = 0;
            var edgeWeigthCount = 0;

            var rows = distribution.Where(x => x.Value == ChromosomePart.First)
                                              .Select(x => x.Key);

            var cols = distribution.Where(x => x.Value == ChromosomePart.Second)
                                              .Select(x => x.Key);
            foreach (var row in rows)
            {
                foreach (var col in cols)
                {
                    if (matrix.Elements[row][col] >= 1)
                    {
                        edgeCount++;
                        edgeWeigthCount += matrix.Elements[row][col];
                    }
                }
            }

            return new Dictionary<ChromosomeFactor, int>
            {
                [ChromosomeFactor.EdgeCount] = edgeCount,
                [ChromosomeFactor.ConnectedEdgeWeigthSum] = edgeWeigthCount,
                [ChromosomeFactor.ConnectedEdgeWeigthSum | ChromosomeFactor.EdgeCount] = edgeWeigthCount + edgeCount
            };
        }

        public IChromosome FlipNodeOnChromosoe(IChromosome chromosome, int maxDiffBetweenNode, IMatrix matrix)
        {
            var temp = new Chromosome
            {
                Distribution = chromosome.Distribution,
                Factors = chromosome.Factors,
                Id = chromosome.Id
            };
            var random = new Random();
            var maxIteration = 20;
            var currentIteration = 0;
            do
            {
                int left;
                int rigth;
                do
                {
                    (left, rigth) = RandomNumberGeneratorUtils.GenerateTwoRandomNumbers(random, 0, chromosome.Distribution.Count());

                } while ((chromosome.Distribution[left] == ChromosomePart.First && chromosome.Distribution[rigth] == ChromosomePart.Second) == false);

                chromosome.Distribution[left] = ChromosomePart.Second;
                chromosome.Distribution[rigth] = ChromosomePart.First;

                chromosome.Factors = GetChromosomeFactors(chromosome.Distribution, matrix);

                currentIteration++;

                if (IsNodeCountValid(chromosome.Distribution, maxDiffBetweenNode) == false)
                {
                    chromosome = temp;
                }
                else
                {
                    break;
                }

                if (currentIteration > maxIteration)
                {
                    chromosome = temp;
                    throw new Exception("Max iteration reach after flipping nodes, consider change max Diff Between Nodes");
                }

            } while (IsNodeCountValid(chromosome.Distribution, maxDiffBetweenNode) == false);

            chromosome.Factors = GetChromosomeFactors(chromosome.Distribution, matrix);
            return chromosome;
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

            } while (IsNodeCountValid(distribution, maxDiffBetweenNode) == false);

            return new Chromosome
            {
                Id = Guid.NewGuid(),
                Distribution = distribution,
                Factors = GetChromosomeFactors(distribution, matrix)
            };
        }
    }
}
