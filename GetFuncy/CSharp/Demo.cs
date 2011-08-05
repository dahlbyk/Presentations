using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Solutionizing.Funcy.CSharp
{
    static class Demo
    {
        public static void DelegateTest()
        {
            Expression<Func<int, string>> intToString =
                (i) => new string(i.ToString().Reverse().ToArray());

            Console.WriteLine(intToString);
            Console.WriteLine(intToString.Compile()(1234));

            Action<int, int> intsToString =
                (int i, int j) =>
                    {
                        var reversed = (i + j).ToString().Reverse();
                        var ra = reversed.ToArray();
                        Console.WriteLine(new string(ra));
                    };

            intsToString(1200, 34);
        }

        static IEnumerable<int> Values(this Random random)
        {
            while (true)
                yield return random.Next();
        }

        static void TestRandom()
        {
            var random = new Random();

            var tenRand = random.Values().Take(10);

            var tenOddRand = random.Values().Where(x => x % 2 == 1).Take(10);

            var cards = new[] { "HK", "HQ", "C9" };
            var sortedDeck = cards
                .Zip(random.Values(), (c, r) => new { Card = c, SortOrder = r })
                .OrderBy(z => z.SortOrder);
            sortedDeck.Dump();
        }

        static void Dump<T>(this IEnumerable<T> source)
        {
            foreach (var s in source)
                Console.WriteLine(s);
        }
    }
}
