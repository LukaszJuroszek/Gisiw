using Graph.Core.Models;
using Graph.Core.Services;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Graph.Core.Tests
{
    [TestFixture]
    public class ChromosomeServiceUt
    {
        private IChromosomeService _sut;
        private readonly int _maxDiffBetweenNode = 3;

        [SetUp]
        public void Setup()
        {
            _sut = new ChromosomeService();
        }

        [Test, Timeout(200)]
        [Repeat(100)]
        public void GenerateChromosome_Should_Pass()
        {
            //Arange
            //Act
            var result = _sut.GenerateChromosome(new Matrix(MatrixHelper.BasicMatrix5By5), _maxDiffBetweenNode);

            //Assert
            Assert.That(_sut.IsNodeCountValid(result.Distribution, _maxDiffBetweenNode) == true);
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

        [Test]
        [TestCaseSource(typeof(ChrpomosoeDistributionData), nameof(ChrpomosoeDistributionData.TestCases))]
        public bool IsNodeCountValid_Should_Count_Nodes_As_Expected(Chromosome chromosome, int maxDiffBetweenNode)
        {
            return _sut.IsNodeCountValid(chromosome.Distribution, maxDiffBetweenNode);
        }

        [Test]
        [TestCaseSource(typeof(ChrpomosoeDistributionDataWithMatrix), nameof(ChrpomosoeDistributionData.TestCases))]
        public (int edgeCount, int connectedEdgeWeigthSum) GetConnectedEdgeCountAndWegithCount_Should_Count_Nodes_As_Expected(Chromosome chromosome, Matrix matrix)
        {
            var result = _sut.GetChromosomeFactors(chromosome.Distribution, matrix);
            return (result[ChromosomeFactor.EdgeCount], result[ChromosomeFactor.ConnectedEdgeWeigthSum]);
        }

        public class ChrpomosoeDistributionData
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.First,
                            [3] = ChromosomePart.First,
                            [4] = ChromosomePart.First,
                        }
                    }, 3).Returns(false);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.First,
                            [3] = ChromosomePart.First,
                            [4] = ChromosomePart.Second,
                        }
                    }, 3).Returns(false);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.First,
                            [3] = ChromosomePart.Second,
                            [4] = ChromosomePart.Second,
                        }
                    }, 3).Returns(true);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.Second,
                            [3] = ChromosomePart.Second,
                            [4] = ChromosomePart.Second,
                        }
                    }, 3).Returns(true);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.Second,
                            [2] = ChromosomePart.Second,
                            [3] = ChromosomePart.Second,
                            [4] = ChromosomePart.Second,
                        }
                    }, 3).Returns(false);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.Second,
                            [1] = ChromosomePart.Second,
                            [2] = ChromosomePart.Second,
                            [3] = ChromosomePart.Second,
                            [4] = ChromosomePart.Second,
                        }
                    }, 3).Returns(false);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.First,
                            [3] = ChromosomePart.First,
                        }
                    }, 3).Returns(false);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.First,
                            [3] = ChromosomePart.Second,
                        }
                    }, 3).Returns(true);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.Second,
                            [3] = ChromosomePart.Second,
                        }
                    }, 3).Returns(true);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.Second,
                            [2] = ChromosomePart.Second,
                            [3] = ChromosomePart.Second,
                        }
                    }, 3).Returns(true);
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.Second,
                            [0] = ChromosomePart.Second,
                            [1] = ChromosomePart.Second,
                            [2] = ChromosomePart.Second,
                        }
                    }, 3).Returns(false);
                }
            }
        }

        public class ChrpomosoeDistributionDataWithMatrix
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.First,
                            [3] = ChromosomePart.First,
                        }
                    }, new Matrix(MatrixHelper.BasicHalfIdentityMatrix4By4)).Returns((0, 0));
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.First,
                            [3] = ChromosomePart.Second,
                        }
                    }, new Matrix(MatrixHelper.BasicHalfIdentityMatrix4By4)).Returns((3, 3));
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.First,
                            [1] = ChromosomePart.First,
                            [2] = ChromosomePart.Second,
                            [3] = ChromosomePart.Second,
                        }
                    }, new Matrix(MatrixHelper.BasicHalfIdentityMatrix4By4)).Returns((4, 4));
                    yield return new TestCaseData(new Chromosome
                    {
                        Distribution = new Dictionary<int, ChromosomePart>
                        {
                            [0] = ChromosomePart.Second,
                            [1] = ChromosomePart.Second,
                            [2] = ChromosomePart.Second,
                            [3] = ChromosomePart.Second,
                        }
                    }, new Matrix(MatrixHelper.BasicHalfIdentityMatrix4By4)).Returns((0, 0));
                }
            }
        }
    }
}
