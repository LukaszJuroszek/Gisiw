using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    [TestFixture]
    public class GraphConsistentServiceUt
    {
        private IGraphConsistentService _sut;
        private IMatrixService _matrixService;

        [SetUp]
        public void Setup()
        {
            _matrixService = new MatrixService();
            _sut = new GraphConsistentService(_matrixService);
        }

        [Test]
        public void IsConsistent_Should_Pass()
        {
            //Arange
            var matrix = _matrixService.GenerateMatrix(10, 1d);
            //Act
            var result = _sut.IsConsistent(matrix);
            //Assert
            Assert.That(result == true);
        }

        [Test]
        public void IsConsistent_Should_Fail_If_No_Connection()
        {
            //Arange
            var matrix = _matrixService.GenerateMatrix(10, 0d);
            //Act
            var result = _sut.IsConsistent(matrix);
            //Assert
            Assert.That(result == false);
        }
    }
}
