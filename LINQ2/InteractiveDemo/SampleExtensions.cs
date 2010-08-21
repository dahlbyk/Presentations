using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutionizing.InteractiveDemo
{
    public static class SampleExtensions
    {
        public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return source
                .Do(x => Console.WriteLine("Pairwise source: {0}", x))
                .Memoize(2)
                .Do(x => Console.WriteLine("Pairwise memoized: {0}", x))
                .Let(m => m.Zip(m.Skip(1), resultSelector));
        }
    }
}
