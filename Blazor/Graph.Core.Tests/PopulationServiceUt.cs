using Graph.Core.Models;
using Graph.Core.Services;
using NSubstitute;
using NUnit.Framework;

namespace Graph.Core.Tests
{
    public class PopulationServiceUt
    {
        private IPopulationService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = Substitute.For<IPopulationService>();
        }

        [Test]
        public void Initialize_Should_Pass()
        {
            //Arange
            //Act
            //_sut.Initialize(1, new MatrixModel(), 1d, 1);
            //Assert
            Assert.Pass();
        }
    }
}
