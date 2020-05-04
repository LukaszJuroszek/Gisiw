using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    using Graph.Core.Models;
    using System.Linq;

    public class PopulationServiceUt
    {
        private IPopulationService _sut;
        private IChromosomeService _chromosomeService;
        private IMatrixService _matrixService;
        private readonly int _maxDiffBetweenNode = 3;

        [SetUp]
        public void Setup()
        {
            _matrixService = new MatrixService(new GraphConsistentService());
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

        [Test]
        public void GenerateChromosome_Should_Have_3_Types_Of_Factors()
        {
            //Arange
            //Act
            var result = _sut.GenerateChromosome(new Matrix(MatrixHelper.BasicMatrix5By5), _maxDiffBetweenNode);

            //Assert
            Assert.That(result.Factors.Keys.Count, Is.EqualTo(3));
        }

        [Test]
        [Repeat(100)]
        public void GenerateChromosome_Factors_Should_Have_One_Sum_From_Two_Factors()
        {
            //Arange
            //Act
            var result = _sut.GenerateChromosome(new Matrix(MatrixHelper.BasicMatrix5By5), _maxDiffBetweenNode);
            var factor1 = result.Factors[ChromosomeFactor.EdgeCount];
            var factor2 = result.Factors[ChromosomeFactor.ConnectedEdgeWeigthSum];
            var factorSum = factor1 + factor2;
            var currentSumFactor = result.Factors[ChromosomeFactor.ConnectedEdgeWeigthSum | ChromosomeFactor.EdgeCount];
            //Assert
            Assert.That(currentSumFactor, Is.EqualTo(factorSum));
        }
    }
}
