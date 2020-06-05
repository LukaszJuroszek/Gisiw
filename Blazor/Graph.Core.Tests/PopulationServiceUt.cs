using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    public class PopulationServiceUt
    {
        private IPopulationService _sut;
        private IChromosomeService _chromosomeService;
        private IMatrixService _matrixService;

        [SetUp]
        public void Setup()
        {
            _matrixService = new MatrixService(new GraphConsistentService());
            _chromosomeService = new ChromosomeService();
            _sut = new PopulationService(_chromosomeService);
        }
    }
}
