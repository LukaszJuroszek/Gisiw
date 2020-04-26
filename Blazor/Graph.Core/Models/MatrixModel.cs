namespace Graph.Core.Models
{
    public class MatrixModel
    {
        public int[][] Elements { get; }

        public MatrixModel(int[][] elements)
        {
            Elements = elements;
        }
    }
}
