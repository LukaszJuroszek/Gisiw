using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Models
{
    public interface IPopulation
    {
        IEnumerable<IChromosome> Members { get; set; }
        Guid[] GuidMap { get; }
    }

    public class Population : IPopulation
    {
        public IEnumerable<IChromosome> Members { get; set; }
        public Guid[] GuidMap => Members.Select(x => x.Id).ToArray();
    }
}
