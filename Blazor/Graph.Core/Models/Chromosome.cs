﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Models
{
    public interface IChromosome
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
        public int FactorsSum => Factors.Sum(x => x.Value);
    }
}
