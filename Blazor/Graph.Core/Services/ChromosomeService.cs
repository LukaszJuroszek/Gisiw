using Graph.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IChromosomeService
    {
        bool IsNodeCountValid(Dictionary<int, ChromosomePart> distribution, int maxDiffBetweenNode);
        Dictionary<ChromosomeFactor, int> GetChromosomeFactors(Dictionary<int, ChromosomePart> distribution, IMatrix matrix);
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
    }
}
