using Graph.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Services
{
    public interface IPopulationService
    {
        IEnumerable<ChromosomeModel> Initialize(MatrixModel matrix, int populationSize, double probability, int maxDiffBetweenNode);
        ChromosomeModel GenerateChromosome(MatrixModel matrix, double probability, int maxDiffBetweenNode);
    }
    public class PopulationService : IPopulationService
    {
        private readonly int firstPart = 0;
        private readonly int secondPart = 1;
        public IEnumerable<ChromosomeModel> Initialize(MatrixModel matrix, int populationSize, double probability, int maxDiffBetweenNode)
        {
            var result = new HashSet<ChromosomeModel>();
            if (result.Count < populationSize)
            {
                do
                {
                    result.Add(GenerateChromosome(matrix, probability, maxDiffBetweenNode));
                }
                while (result.Count < populationSize);
            }
            return result;
        }

        public ChromosomeModel GenerateChromosome(MatrixModel matrix, double probability, int maxDiffBetweenNode)
        {
            ChromosomeModel result;
            var random = new Random();
            do
            {
                result = new ChromosomeModel
                {
                    Elements = new ChromosomeElement[matrix.Elements.Length]
                };
                for (var i = 0; i < matrix.Elements.Length; i++)
                {
                    result.Elements[i] = new ChromosomeElement
                    {
                        NodeNumber = i,
                        ChromosomePartNumber = random.NextDouble() < probability ? firstPart : secondPart
                    };
                }
                var (edgeCount, edgeWeigthCount) = GetConnectedEdgeCountAndWegithCount(result, matrix);
                result.FactorSum1 = edgeCount;
                result.FactorSum2 = edgeWeigthCount;

            } while (IsNodeCountValid(result, maxDiffBetweenNode) == false);

            return result;
        }

        private (int edgeCount, int edgeWeigthCount) GetConnectedEdgeCountAndWegithCount(ChromosomeModel chromosome, MatrixModel matrix)
        {
            var edgeCount = 0;
            var edgeWeigthCount = 0;
            for (var i = 0; i < matrix.Elements.Length; i++)
            {
                var rowElem = chromosome.Elements.FirstOrDefault(n => n.ChromosomePartNumber == firstPart && n.NodeNumber == i);
                //first part is selected by user, second is not present, via row
                if (rowElem == null)
                {
                    for (var j = 0; j < matrix.Elements[i].Length; j++)
                    {
                        //check if column is in first present
                        var element = chromosome.Elements.FirstOrDefault(n => n.ChromosomePartNumber == secondPart && n.NodeNumber == j);
                        if (element != null && matrix.Elements[i][j] >= 1)
                        {
                            edgeCount++;
                            edgeWeigthCount += matrix.Elements[i][j];
                        }
                    }
                }
            }
            return (edgeCount, edgeWeigthCount);
        }

        public bool IsNodeCountValid(ChromosomeModel chromosome, int maxDiffBetweenNode)
        {
            var nodeSum = 0;
            for (var i = 0; i < chromosome.Elements.Length; i++)
            {
                var isFirstPart = chromosome.Elements[i].ChromosomePartNumber == firstPart;
                nodeSum = isFirstPart ? nodeSum + 1 : nodeSum;
            }
            var firstPartSum = chromosome.Elements.Length - nodeSum;
            var secondPartSum = chromosome.Elements.Length - firstPartSum;

            return (Math.Abs(firstPartSum - secondPartSum) <= maxDiffBetweenNode);
        }
    }
}
