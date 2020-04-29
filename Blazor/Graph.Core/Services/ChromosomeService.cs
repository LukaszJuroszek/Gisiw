using Graph.Core.Models;
using System;
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

            return Math.Abs(chromosome.Distribution.Count - nodeSum) <= maxDiffBetweenNode;
        }

        public (int edgeCount, int edgeWeigthCount) GetConnectedEdgeCountAndWegithCount(ChromosomeModel chromosome, MatrixModel matrix)
        {
            var edgeCount = 0;
            var edgeWeigthCount = 0;
            for (var nodeNumberByRow = 0; nodeNumberByRow < chromosome.Distribution.Count; nodeNumberByRow++)
            {
                //first part is selected by user, second is not present, via row
                if ((chromosome.Distribution[nodeNumberByRow] == ChromosomePart.First) == false)
                {
                    for (var nodeNumberByColumn = 0; nodeNumberByColumn < matrix.Elements[nodeNumberByRow].Length; nodeNumberByColumn++)
                    {
                        //check if column is in first present
                        if (chromosome.Distribution[nodeNumberByColumn] == ChromosomePart.Second &&
                            matrix.Elements[nodeNumberByRow][nodeNumberByColumn] >= 1)
                        {
                            edgeCount++;
                            edgeWeigthCount += matrix.Elements[nodeNumberByRow][nodeNumberByColumn];
                        }
                    }
                }
            }
            return (edgeCount, edgeWeigthCount);
        }
    }
}
