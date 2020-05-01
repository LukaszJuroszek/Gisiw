using System.Collections.Generic;

namespace Graph.Core.Models
{
    public interface IPopulation
    {
        IEnumerable<IChromosome> Members { get; set; }
    }

    public class Population : IPopulation
    {
        public IEnumerable<IChromosome> Members { get; set; }
    }
}
