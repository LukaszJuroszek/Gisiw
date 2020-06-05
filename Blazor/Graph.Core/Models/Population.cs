using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Core.Models
{
    public interface IPopulation : IDeepCopy<IPopulation>
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

        public IPopulation DeepCopy()
        {
            var list = new List<IChromosome>();

            foreach (var member in Members)
            {
                list.Add(member.DeepCopy());
            }

            return new Population(list);
        }
    }
}
