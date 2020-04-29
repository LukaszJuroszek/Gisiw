using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    [TestFixture]
    public class MatrixServiceUt
    {
        private IMatrixService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new MatrixService();
            //_sut = Substitute.For<IMatrixService>();
        }

        [TestCase(5)]
        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void Initialize_Should_Pass(int nodeCount)
        {
            //Arange
            //Act
            var result = _sut.GenerateMatrix(nodeCount, .5);
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
