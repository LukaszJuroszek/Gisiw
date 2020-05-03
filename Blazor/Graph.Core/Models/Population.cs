using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Models
{
    public interface IPopulation
    {
        IEnumerable<IChromosome> Members { get; }
        Guid[] GuidMap { get; }
    }

    public class Population : IPopulation
    {
        public IEnumerable<IChromosome> Members { get; }
        public Guid[] GuidMap => Members.Select(x => x.Id).ToArray();

        public Population(IEnumerable<IChromosome> members)
        {
            Members = members;
        }
    }
}
