using System;

namespace Tanks
{
    public class DefaultRandom : IRandom
    {
        private readonly Random _random;

        public DefaultRandom(Random random)
        {
            _random = random;
        }

        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }

        public int Next(int max)
        {
            return _random.Next(max);
        }
    }
}