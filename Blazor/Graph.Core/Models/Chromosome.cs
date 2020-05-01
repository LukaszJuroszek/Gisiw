using System.Collections.Generic;

namespace Graph.Core.Models
{
    public interface IChromosome
    {
        Dictionary<int, ChromosomePart> Distribution { get; set; }
        int FactorSum1 { get; set; }
        int FactorSum2 { get; set; }
    }

    public enum ChromosomePart
    {
        Unknown = 0,
        First = 1 << 0,
        Second = 1 << 2,
    }

    public class Chromosome : IChromosome
    {
        public Dictionary<int, ChromosomePart> Distribution { get; set; }
        public int FactorSum1 { get; set; }
        public int FactorSum2 { get; set; }
    }

}
