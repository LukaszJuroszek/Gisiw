namespace Graph.Core.Models
{
    public class ChromosomeModel
    {
        private readonly int iterationNumber = 0;
        public ChromosomeElement[] Elements { get; set; }
        public int FactorSum1 { get; set; }
        public int FactorSum2 { get; set; }
    }
}
