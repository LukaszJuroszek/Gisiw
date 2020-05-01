using Graph.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IChromosomeService
    {
        bool IsNodeCountValid(ChromosomeModel chromosome, int maxDiffBetweenNode);
        (int edgeCount, int edgeWeigthCount) GetConnectedEdgeCountAndWegithCount(ChromosomeModel chromosome, MatrixModel matrix);
    }

    public class ChromosomeService : IChromosomeService
    {
        public bool IsNodeCountValid(ChromosomeModel chromosome, int maxDiffBetweenNode)
        {
            var nodeSum = chromosome.Distribution.Count(x => x.Value == ChromosomePart.First);
            var firstPart = chromosome.Distribution.Count - nodeSum;
            var secondPart = chromosome.Distribution.Count - firstPart;
            return Math.Abs(firstPart - secondPart) < maxDiffBetweenNode;
        }

        public (int edgeCount, int edgeWeigthCount) GetConnectedEdgeCountAndWegithCount(ChromosomeModel chromosome, MatrixModel matrix)
        {
            var edgeCount = 0;
            var edgeWeigthCount = 0;

            var rows = chromosome.Distribution.Where(x => x.Value == ChromosomePart.First)
                                              .Select(x => x.Key);

            var cols = chromosome.Distribution.Where(x => x.Value == ChromosomePart.Second)
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

            return (edgeCount, edgeWeigthCount);
        }
    }
}
