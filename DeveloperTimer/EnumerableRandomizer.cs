using System;
using System.Collections.Generic;
using System.Linq;

namespace DeveloperTimer
{
    public class EnumerableRandomizer
    {
        private readonly GetRandomNumber randomNumberGenerator;

        public EnumerableRandomizer()
        {
            var random = new Random();
            randomNumberGenerator = max => random.Next(0, max);
        }

        public EnumerableRandomizer(GetRandomNumber randomNumberGenerator)
        {
            this.randomNumberGenerator = randomNumberGenerator;
        }

        public IEnumerable<T> Randomize<T>(IEnumerable<T> values)
        {
            var list = new List<T>(values);
            var result = new List<T>();

            while (list.Any())
            {
                var ptr = randomNumberGenerator(list.Count - 1);
                result.Add(list[ptr]);

                list.RemoveAt(ptr);
            }

            return result;
        }
    }
}