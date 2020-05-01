using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    using Graph.Core.Models;
    public class PopulationServiceUt
    {
        private IPopulationService _sut;
        private IChromosomeService _chromosomeService;
        private readonly int _maxDiffBetweenNode = 3;

        [SetUp]
        public void Setup()
        {
            _chromosomeService = new ChromosomeService();
            _sut = new PopulationService(_chromosomeService);
        }

        [Test, Timeout(200)]
        [Repeat(100)]
        public void GenerateChromosome_Should_Pass()
        {
            //Arange
            //Act
            var result = _sut.GenerateChromosome(new Matrix(MatrixHelper.BasicMatrix5By5), _maxDiffBetweenNode);
            //Assert
            Assert.That(_chromosomeService.IsNodeCountValid(result.Distribution, _maxDiffBetweenNode) == true);
        }

    }
}
