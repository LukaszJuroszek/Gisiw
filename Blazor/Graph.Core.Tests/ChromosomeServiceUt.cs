using Graph.Core.Services;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    [TestFixture]
    public class ChromosomeServiceUt
    {
        private IChromosomeService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ChromosomeService();
        }

        [Test]
        public void IsNodeCountValid_Should_Count_Nodes_As_Expected()
        {
            //Arange
            //Act
            //_sut.IsNodeCountValid();
            //Assert
            Assert.Pass();
        }
    }
}
