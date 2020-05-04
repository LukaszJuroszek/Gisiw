using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    [TestFixture]
    public class GraphConsistentServiceUt
    {
        private double _probability = 0.5d;
        private IGraphConsistentService _sut;
        private IMatrixService _matrixService;

        [SetUp]
        public void Setup()
        {
            _sut = new GraphConsistentService();
            _matrixService = new MatrixService(_sut);
        }

        [Test]
        [Repeat(100)]
        public void IsConsistent_Should_Alwasy_Pass()
        {
            //Arange
            var matrix = _matrixService.GenerateMatrix(10, _probability);
            //Act
            var result = _sut.IsConsistent(matrix.Elements);
            //Assert
            Assert.That(result == true);
        }
    }
}
