using System.Collections.Generic;

namespace Numeripack
{
    public static class ExtensionMethods
    {
        public static int Factorial(this int number)
        {
            if (number == 0)
                return 1;

            return number * Factorial(number - 1);
        }

        public static int Pow(this int x, int y)
        {
            var result = 1;
            while (y != 0)
            {
                if ((y & 1) == 1)
                    result *= x;
                x *= x;
                y >>= 1;
            }
            return result;
        }

        public static IEnumerable<TSource> SkipRepetitions<TSource>(this IEnumerable<TSource> source)
        {
            var isFirst = true;
            TSource previousElement = default;

            foreach (var element in source)
            {
                if (isFirst)
                {
                    isFirst = false;
                    previousElement = element;
                    yield return element;
                }
                else if (!previousElement.Equals(element))
                {
                    previousElement = element;
                    yield return element;
                }
            }
        }
    }
}
