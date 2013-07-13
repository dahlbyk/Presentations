using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace Solutionizing.Demo.LinqInternals
{
    static class Program
    {
        static void Main(string[] args)
        {
            #region Data

            var States = new[] {
                new { State = "Iowa", Abbr = "IA", Population = 3002555L,
                    Cities = new [] { "Des Moines", "Cedar Rapids", "Omaha" } },
                new { State = "Missouri", Abbr = "MO", Population = 5911605L,
                    Cities = new[] { "Kansas City", "St. Louis", "Springfield" } },
                new { State = "Illinois", Abbr = "IL", Population = 12901563L,
                    Cities = new string[] { "Chicago", "Rockford", "Joliet" } }
            };
            var AreaCodes = new[] {
                new { StateAbbr = "IA", AreaCode = 515 },
                new { StateAbbr = "IA", AreaCode = 319 },
                new { StateAbbr = "IA", AreaCode = 563 },
                new { StateAbbr = "IA", AreaCode = 641 },
                new { StateAbbr = "IA", AreaCode = 712 },
                new { StateAbbr = "MO", AreaCode = 314 },
                new { StateAbbr = "MO", AreaCode = 417 },
                new { StateAbbr = "MO", AreaCode = 573 },
                new { StateAbbr = "MO", AreaCode = 636 },
                new { StateAbbr = "MO", AreaCode = 660 },
                new { StateAbbr = "MO", AreaCode = 816 }
            };

            var QStates = States.AsQueryable();
            var QAreaCodes = AreaCodes.AsQueryable();

            #endregion

            //Expression<Func<int, int>> inc 
            //    = a => a + 1;
            //inc.ToString().Write();
            //var compiled = inc.Compile();
            //compiled(2).Write();

            //var y = new { Hi = "Hello" };
            //y.ToString().Write();

            var r2 = from s in States
                     from i in s.GetIndex()
                     select string.Format("{0} {1}", i, s.State);

            var r3 = from x in States.Select((s,i) => new {s,i})
                     select string.Format("{0} {1}", x.i, x.s.State);

            int ii = 0;
            foreach (var s in States)
            {
                string.Format("{0} {1}", ii, s.State).Write();
                ii++;
            }

            States.Select((s, i) => string.Format("{0} {1}", i, s.State));

            States[0].Cities.SelectMany(c => c.GetIndex(), (c, i) => new { c, i });
            r2.ForEach(Write);

            var r = from s in States
                    join a in AreaCodes
                      on s.Abbr equals a.StateAbbr
                    // where s.Population > 5000000L
                    orderby s.Population descending
                    orderby a.AreaCode
                    select new { s.State, AreaCode = a == null ? null : (int?)a.AreaCode };

            var min = States.Aggregate((x, y) => x.Population < y.Population ? x : y);
            min.Write();

            var sum = States.Aggregate(0L, (acc, s) => acc + s.Population);
            States.Aggregate(0L, (acc, s) => acc + 1).Write();

            States.Sum(s => s.Population).Write();

            Enumerable.Range(0, 10).Sum().Write();

            var nameList = States.Aggregate(new StringBuilder(),
                (sb, s) => sb.AppendLine(s.State));
            nameList.Write();

            r.ForEach(x => Write(x));

            Console.ReadKey();
        }

        #region Extension Methods

        static void Write<T>(this T @this)
        {
            Console.WriteLine(@this);
        }

        static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T t in source)
                action(t);
        }

        static SelectIndexProvider GetIndex<T>(this T @this)
        {
            return null;
        }

        public class SelectIndexProvider { }

        static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, SelectIndexProvider> collectionSelector,
            Func<TSource, int, TResult> resultSelector)
        {
            return source.Select(resultSelector);
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, int, TResult> selector)
        {
            int i = 0;
            foreach (var s in @this)
                yield return selector(s, i++);
        }

        #endregion

        #region Beyond

        public static IEnumerable<T> Use<T>(this T obj) where T : IDisposable
        {
            Guid g = default(Guid);
            try
            {
                g = Guid.NewGuid();
                Console.WriteLine("Yielding " + g);
                yield return obj;
            }
            finally
            {
                Console.WriteLine("Disposing " + g);
                if (obj != null)
                    obj.Dispose();
            }
        }

        //public static IEnumerable<TResult> SelectMany<TSource, TStream, TResult>(
        //    this IEnumerable<TSource> source,
        //    Func<TSource, TStream> collectionSelector,
        //    Func<TSource, TStream, TResult> resultSelector)
        //    where TStream : Stream
        //{
        //    foreach (var s in source)
        //    {
        //        TStream obj = default(TStream);
        //        Guid g = default(Guid);
        //        try
        //        {
        //            obj = collectionSelector(s);
        //            g = Guid.NewGuid();
        //            Console.WriteLine("Yielding " + g);
        //            yield return resultSelector(s, obj);
        //        }
        //        finally
        //        {
        //            Console.WriteLine("Disposing " + g);
        //            if (obj != null)
        //                obj.Dispose();
        //        }
        //    }
        //}

        public static void WriteFiles1(string dirPath)
        {
            (
                from path in Directory.GetFiles(dirPath)
                from fs in File.OpenRead(path).Use()
                select new { path, fs.Length }
            ).ForEach(Write);
        }

        //public static void WriteFiles2(string dirPath)
        //{
        //    (
        //        from path in Directory.GetFiles(dirPath)
        //        from fs in File.OpenRead(path)
        //        select new { path, fs.Length }
        //    ).ForEach(Write);
        //}

        #endregion
    }
}
