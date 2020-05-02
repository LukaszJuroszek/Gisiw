using Graph.Core.Models;
using Graph.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IChromosomeService
    {
        bool IsNodeCountValid(Dictionary<int, ChromosomePart> distribution, int maxDiffBetweenNode);
        Dictionary<ChromosomeFactor, int> GetChromosomeFactors(Dictionary<int, ChromosomePart> distribution, IMatrix matrix);
        IChromosome FlipNodeOnChromosoe(IChromosome chromosome, int maxDiffBetweenNode, IMatrix matrix);
    }

    public class ChromosomeService : IChromosomeService
    {
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
                [ChromosomeFactor.ConnectedEdgeWeigthSum] = edgeWeigthCount
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
            var maxIteration = 20;
            var currentIteration = 0;
            do
            {
                int left;
                int rigth;
                do
                {
                    (left, rigth) = RandomNumberGeneratorUtils.GenerateTwoRandomNumbers(0, chromosome.Distribution.Count());

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
    }
}
