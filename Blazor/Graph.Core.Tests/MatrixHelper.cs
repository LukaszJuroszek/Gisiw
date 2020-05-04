namespace Graph.Core.Tests
{
    public static class MatrixHelper
    {
        public static int[][] BasicMatrix5By5 { get; set; } = new int[5][]
        {
            new int[5] {0,1,1,1,1},
            new int[5] {0,0,1,1,1},
            new int[5] {0,0,0,1,1},
            new int[5] {0,0,0,0,1},
            new int[5] {0,0,0,0,0}
        };

        public static int[][] BasicHalfIdentityMatrix4By4 { get; set; } = new int[4][]
       {
            new int[4] {0,1,1,1},
            new int[4] {0,0,1,1},
            new int[4] {0,0,0,1},
            new int[4] {0,0,0,0},
       };
    }
}
