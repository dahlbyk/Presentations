using System;
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
            var nums = from n in Enumerable.Range(1, 100)
                       select n.ToString()
                           into s

                           where s[0] == '1'
                           group s by s.Last()
                               into g

                               orderby g.Min(s => s.Length) descending, g.Key
                               select new
                               {
                                   g.Key,
                                   N = string.Join(", ", g)
                               };

            nums.Dump();
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
