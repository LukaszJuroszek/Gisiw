using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    using Graph.Core.Models;
    public class PopulationServiceUt
    {
        private IPopulationService _sut;
        private IChromosomeService _chromosomeService;
        private readonly double _probability = 0.9d;
        private readonly int _maxDiffBetweenNode = 3;

        [SetUp]
        public void Setup()
        {
            _chromosomeService = new ChromosomeService();
            _sut = new PopulationService(_chromosomeService);
        }

        [Test, Timeout(2000)]
        public void GenerateChromosome_Should_Pass()
        {
            //Arange
            //Act
            var result = _sut.GenerateChromosome(new MatrixModel(MatrixHelper.BasicMatrix5By5), _probability, _maxDiffBetweenNode);
            //Assert
            Assert.That(_chromosomeService.IsNodeCountValid(result, _maxDiffBetweenNode) == true);
        }

    }
}
