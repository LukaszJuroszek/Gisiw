using System;

namespace Graph.Core.Utils
{
    public static class RandomNumberGeneratorUtils
    {
        public static (int left, int rigth) GenerateTwoRandomNumbers(int minValue, int maxValue)
        {
            var left = -2;
            var rigth = -1;
            do
            {
                left = GenerateRandomNumbers(minValue, maxValue);
                rigth = GenerateRandomNumbers(minValue, maxValue);
            } while (left == rigth);

            return (left, rigth);
        }

        public static int GenerateRandomNumbers(int minValue, int maxValue) => new Random().Next(minValue, maxValue);
    }
}
