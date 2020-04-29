namespace Graph.Core.Tests
{
    public static class MatrixHelper
    {
        private const int nodeNumber = 5;
        public static int[][] BasicMatrix { get; set; } = new int[nodeNumber][]
        {
            new int[nodeNumber] {0,1,1,1,1 },
            new int[nodeNumber] {0,0,1,1,1 },
            new int[nodeNumber] {0,0,0,1,1 },
            new int[nodeNumber] {0,0,0,0,1 },
            new int[nodeNumber] {0,0,0,0,0 }
        };
    }
}
