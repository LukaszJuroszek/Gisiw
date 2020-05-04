using System;

namespace Graph.Core.Utils
{
    public static class RandomNumberGeneratorUtils
    {
        public static (int left, int rigth) GenerateTwoRandomNumbers(Random random,int minValue, int maxValue)
        {
            var left = -2;
            var rigth = -1;
            do
            {
                left = random.Next(minValue, maxValue);
                rigth = random.Next(minValue, maxValue);
            } while (left == rigth);

            return (left, rigth);
        }
    }
}
