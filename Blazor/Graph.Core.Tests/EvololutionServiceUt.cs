using Graph.Core.Models;
using Graph.Core.Services;
using NUnit.Framework;
using System;
using System.Linq;

namespace Graph.Core.Tests
{
    [TestFixture]
    public class EvololutionServiceUt
    {
        private const int numberOfTournamentRounds = 100;
        private IEvolutionService _sut;
        private IChromosomeService _chromosomeService;
        private IMatrixService _matrixService;
        private IPopulationService _populationService;

        [SetUp]
        public void Setup()
        {
            _chromosomeService = new ChromosomeService();
            _populationService = new PopulationService(_chromosomeService);
            _matrixService = new MatrixService(new GraphConsistentService());
            _sut = new EvolutionService(_chromosomeService);
        }

        [Test]
        [Repeat(50)]
        public void GetBestChromosomes_Count_Should_Be_Same_As_Turnament_Count()
        {
            //Arange
            var matrix = _matrixService.GenerateMatrix(10, 0.5);
            var population = _populationService.Initialize(matrix, 50, 3);
            //Act
            var result = _sut.GetBestChromosomes(ChromosomePart.First, population.Population, numberOfTournamentRounds).Count();
            //Assert
            Assert.That(result, Is.EqualTo(numberOfTournamentRounds));
        }

        [Test]
        [Repeat(50)]
        public void RunIteration_Should_Not_Throw_Any_Exception()
        {
            //Arange
            var matrix = _matrixService.GenerateMatrix(10, 0.5);
            var population = _populationService.Initialize(matrix, 50, 3);
            //Act
            //Assert
            Assert.DoesNotThrow(() => { _sut.RunIteration(population, matrix, 3); });
        }

        [Test]
        public void RunIteration_Should_Return_Member_Count()
        {
            //Arange
            var matrix = _matrixService.GenerateMatrix(10, 0.5);
            var population = _populationService.Initialize(matrix, 50, 3);
            //Actb
            var result = _sut.RunIteration(population,matrix, numberOfTournamentRounds).Population.Members.Count();
            //Assert
            Assert.That(result, Is.EqualTo(numberOfTournamentRounds));
        }

        public void RunIteration_Should_Pass()
        {
            //Arange
            var matrix = _matrixService.GenerateMatrix(10, 0.5);
            var population = _populationService.Initialize(matrix, 50, 3);
            //Actb
            var result = _sut.RunIteration(population, matrix, numberOfTournamentRounds);
            //Assert
            Assert.That(result, Is.EqualTo(numberOfTournamentRounds));
        }
    }
}