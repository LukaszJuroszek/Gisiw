namespace Graph.Core.Utils
{
    public static class ProbabilityUtils
    {
        public static double AdaptProbability(this double probability, int tryCount)
        {
            if (tryCount % 20 == 0 && tryCount != 0)
            {
                if (probability < 0.5d && probability + 0.1d < 1d)
                {
                    return probability += 0.1d;
                }
                else if (probability > 0.5d && probability - 0.1d > 0d)
                {
                    return probability -= 0.1d;
                }
            }

            return probability;
        }
    }
}
