namespace Graph.Core.Models
{
    public interface IMatrix
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
    }
}
