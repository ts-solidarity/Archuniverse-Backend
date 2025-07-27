using System;

namespace Archuniverse.Utilities
{
    public static class RandomProvider
    {
        private static readonly Random _rng = new Random();

        // Random int
        public static int Next() => _rng.Next();
        public static int Next(int max) => _rng.Next(max);
        public static int Next(int min, int max) => _rng.Next(min, max);

        // Random double [0.0, 1.0)
        public static double NextDouble() => _rng.NextDouble();

        // Random float [0.0f, 1.0f)
        public static float NextFloat() => (float)_rng.NextDouble();

        // Random float in range [min, max)
        public static float NextFloat(float min, float max)
        {
            if (min > max)
                throw new ArgumentException("min must be <= max");
            return min + (float)_rng.NextDouble() * (max - min);
        }

        // Random bool (50/50)
        public static bool NextBool() => _rng.Next(2) == 0;

        // Weighted bool (0.0 to 1.0 chance for true)
        public static bool NextBool(double trueChance)
        {
            if (trueChance <= 0) return false;
            if (trueChance >= 1) return true;
            return _rng.NextDouble() < trueChance;
        }

        // Random element from array
        public static T NextFrom<T>(T[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array must not be null or empty.");
            return array[_rng.Next(array.Length)];
        }
    }
}
