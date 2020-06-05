using Graph.Core.Models;
using StackExchange.Profiling;
using System.Collections.Generic;

namespace Graph.Core.Services
{
    public interface IPopulationService
    {
        IInitializedPopulationResult Initialize(IMatrix matrix, int populationSize, int maxDiffBetweenNode);
    }

    public class PopulationService : IPopulationService
    {
        private readonly MiniProfiler _profiler;
        private readonly IChromosomeService _chromosomeService;

        public PopulationService(IChromosomeService chromosomeService)
        {
            _profiler = MiniProfiler.StartNew(nameof(PopulationService));
            _chromosomeService = chromosomeService;
        }

        public IInitializedPopulationResult Initialize(IMatrix matrix, int populationSize, int maxDiffBetweenNode)
        {
            var result = new HashSet<IChromosome>();
            using (_profiler.Step(nameof(Initialize)))
            {
                if (result.Count < populationSize)
                {
                    do
                    {
                        result.Add(_chromosomeService.GenerateChromosome(matrix, maxDiffBetweenNode));
                    }
                    while (result.Count < populationSize);
                }
            }
            //_profiler.Stop();
            //Console.WriteLine(_profiler.RenderPlainText());
            return new InitializedPopulationResult(new Population(result));
        }
       
    }
}
