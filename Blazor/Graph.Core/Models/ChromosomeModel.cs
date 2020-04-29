using System.Collections.Generic;

namespace Graph.Core.Models
{
    public class ChromosomeModel
    {
        public Dictionary<int, ChromosomePart> Distribution { get; set; }
        public int FactorSum1 { get; set; }
        public int FactorSum2 { get; set; }
    }

    public enum ChromosomePart
    {
        Unknown = 0,
        First = 1 << 0,
        Second = 1 << 2,
    }
}
