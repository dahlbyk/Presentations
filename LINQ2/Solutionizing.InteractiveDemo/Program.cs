using System;
using System.Collections.Generic;
using System.Linq;

namespace InteractiveDemo
{
    static class Program
    {
        static void Main(string[] args)
        {
            var s = Enumerable.Range(1, 10);

            var r = s.Pairwise((x, y) => new { x, y });

            r.Run(Console.WriteLine);

            s.Prune(z => z.Zip(z, (x, y) => new { x, y })).Run(Console.WriteLine);

            Console.ReadKey();
        }

        static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
        {
            if(source == null) throw new ArgumentNullException("source");
            if(resultSelector == null) throw new ArgumentNullException("resultSelector");

            return source
                .Do(x => Console.WriteLine("Pairwise source: {0}", x))
                .Memoize(2)
                //.Do(x => Console.WriteLine("Pairwise memoized: {0}", x))
                .Let(m => m.Zip(m.Skip(1), resultSelector));
        }
    }
}
