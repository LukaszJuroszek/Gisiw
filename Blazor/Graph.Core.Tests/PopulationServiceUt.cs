using Graph.Core.Models;
using Graph.Core.Services;
using NSubstitute;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    using Graph.Core.Models;
    public class PopulationServiceUt
    {
        private IPopulationService _sut;
        private IChromosomeService chromosomeService;
        private readonly double probability = 0.5d;
        private readonly int maxDiffBetweenNode = 3;

        [SetUp]
        public void Setup()
        {
            chromosomeService =  Substitute.For<IChromosomeService>();
            _sut = new PopulationService(chromosomeService);
        }

        [Test]
        public void GenerateChromosome_Should_Pass()
        {
            //Arange
            //Act
            _sut.GenerateChromosome(new MatrixModel(MatrixHelper.BasicMatrix), probability, maxDiffBetweenNode);
            //Assert
            Assert.Pass();
        }

    }
}
