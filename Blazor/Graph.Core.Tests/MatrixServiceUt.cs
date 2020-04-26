using Graph.Core.Models;
using Graph.Core.Services;
using NSubstitute;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    public class MatrixServiceUt
    {
        private IMatrixService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = Substitute.For<IMatrixService>();
        }

        [Test]
        public void Initialize_Should_Pass()
        {
            //Arange
            //Act
            _sut.GenerateMatrix(1, 1d);
            //Assert
            Assert.Pass();
        }
    }
}
