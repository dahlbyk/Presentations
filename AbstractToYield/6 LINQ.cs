using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AbstractToYield
{
    /*  VB LINQ Extras
     *
     *  Aggregate
     *  Distinct
     *  Skip
     *  Take
     */

    public static class Linq
    {
        static void Query()
        {
            // Pretend we don't know the type
            IEnumerable letters = Enumerable.Range('A', 26).Select(l => (char)l);

            var words = new[] { "code", "camp", "C#", "Visual Studio", "LINQ" };

            var queried = from w in words

                          group w by char.ToUpper(w[0])
                              into g

                              join char l in letters
                                  on (g.Key) equals l

                              let wordList = string.Join(", ", g)

                              orderby g.Max(w => w.Length) descending, g.Count()

                              select new
                              {
                                  g.Key,
                                  wordList
                              };

            queried.Dump();
        }

        static void MyLinq()
        {
            var nums = Enumerable.Range(0, 50);

            var filtered = nums.MyWhere(delegate(int n)
                                            {
                                                return n%4 == 1;
                                            });

            filtered.Dump();
        }

        static IEnumerable<int> MyWhere(
            this IEnumerable<int> source, Func<int, bool> predicate)
        {
            foreach (var x in source)
                if (x == 20)
                    yield break;
                else if (predicate(x))
                    yield return x;
        }
    }
}
