using Graph.Core.Services;
using NSubstitute;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    public class EvololutionServiceUt
    {
        private IEvolutionService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = Substitute.For<IEvolutionService>();
        }

        [Test]
        public void Test1()
        {
            //Arange
            //Act
            _sut.mutateChromosomes();
            //Assert
            Assert.Pass();
        }
    }
}