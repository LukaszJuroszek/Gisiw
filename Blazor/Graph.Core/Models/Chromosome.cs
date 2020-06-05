using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Models
{
    public interface IChromosome : IDeepCopy<IChromosome>
    {
        public Guid Id { get; set; }
        Dictionary<int, ChromosomePart> Distribution { get; set; }
        Dictionary<ChromosomeFactor, int> Factors { get; set; }
        int FactorsSum { get; }
    }

    [Flags]
    public enum ChromosomePart
    {
        Unknown = 0,
        First = 1 << 0,
        Second = 1 << 2,
    }

    [Flags]
    public enum ChromosomeFactor
    {
        Unknown = 0,
        EdgeCount = 1 << 0,
        ConnectedEdgeWeigthSum = 1 << 2,
    }

    public class Chromosome : IChromosome
    {
        public Guid Id { get; set; }
        public Dictionary<int, ChromosomePart> Distribution { get; set; }
        public Dictionary<ChromosomeFactor, int> Factors { get; set; }
        public int FactorsSum => Factors.Where(x => x.Key == (ChromosomeFactor.ConnectedEdgeWeigthSum | ChromosomeFactor.EdgeCount)).Sum(x => x.Value);

        public static int MapToLevel(ChromosomePart chromosomePart) => chromosomePart switch
        {
            ChromosomePart.First => 1,
            ChromosomePart.Second => 2,
            _ => 0,
        };

        public IChromosome DeepCopy() =>
        new Chromosome
        {
            Id = Id,
            Distribution = new Dictionary<int, ChromosomePart>(Distribution),
            Factors = new Dictionary<ChromosomeFactor, int>(Factors)
        };
    }
}
