using Graph.Core.Models;
using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    public class MatrixServiceUt
    {
        private IMatrixService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new MatrixService();
            //_sut = Substitute.For<IMatrixService>();
        }

        [Test]
        public void Initialize_Should_Pass()
        {
            //Arange
            //Act
            var result = _sut.GenerateMatrix(30, .5);
            Log2dArray(result.Elements);

            //Assert
            Assert.IsNotNull(result);
            Assert.Pass();
        }

        private static void Log2dArray(int[][] elements)
        {
            for (var i = 0; i < elements.Length; i++)
            {
                for (var j = 0; j < elements[i].Length; j++)
                {
                    TestContext.Out.Write($"{elements[i][j]} ");
                }
                TestContext.Out.WriteLine();
            }
        }
    }
}
