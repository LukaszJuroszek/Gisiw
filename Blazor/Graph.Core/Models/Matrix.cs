namespace Graph.Core.Models
{
    public interface IMatrix : IDeepCopy<IMatrix>
    {
        int[][] Elements { get; }
    }

    public class Matrix : IMatrix
    {
        public int[][] Elements { get; }

        public Matrix(int[][] elements)
        {
            Elements = elements;
        }

        public IMatrix DeepCopy() => new Matrix((int[][])Elements.Clone());
    }
}
