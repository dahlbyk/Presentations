using System;
using System.Collections.Generic;
using System.Linq;
using System.Disposables;

namespace Solutionizing.InteractiveDemo
{
    public class Samples
    {
        private IEnumerable<int> Odds()
        {
            return EnumerableEx.Generate(1, i => true, i => i, i => i + 2);
        }

        private void OddsExample()
        {
            Odds().Skip(2).Take(20).Write();
        }

        private void DeferExample()
        {
            var message = "immediate!";
            var enumerableFactory = new Func<IEnumerable<string>>(
                () => EnumerableEx.Return(message));

            var m = enumerableFactory();
            var d = EnumerableEx.Defer(enumerableFactory);

            message = "deferred!";

            m.Run(x => Console.WriteLine("Immediate? {0}", x));
            d.Run(x => Console.WriteLine("Deferred? {0}", x));
        }

        private void IfExample()
        {
            var message = "immediate!";

            var i =
                message.StartsWith("i") ?
                EnumerableEx.Return("Eye") : 
                EnumerableEx.Return("Nay");

            var d = EnumerableEx.If(
                () => message.StartsWith("i"),
                EnumerableEx.Return("Eye"),
                EnumerableEx.Return("Nay"));

            message = "deferred!";

            i.Select(x => new { Immediate = x }).Write();
            d.Select(x => new { Deferred = x }).Write();
        }

        private void LetExample()
        {
            Odds().Take(7).Log()
                .Let(o =>
                {
                    Console.WriteLine("Sum!");
                    var sum = o.Sum();
                    Console.WriteLine("Product!");
                    var prod = o.Aggregate((x, acc) => x * acc);
                    return EnumerableEx.Return(new { sum, prod });
                })
                .Write();
        }

        private void PublishExample()
        {
            var odds = Odds().Take(7).Log();

            odds.Publish(o =>
                {
                    Console.WriteLine("Sum!");
                    var sum = o.Sum();
                    Console.WriteLine("Product!");
                    var prod = o.Aggregate((x, acc) => x * acc);
                    return EnumerableEx.Return(new { sum, prod });
                })
                .Write();
        }

        private void ReplayExample()
        {
            Odds().Take(7).Log()
                .Replay(o =>
                {
                    Console.WriteLine("Sum!");
                    var sum = o.Sum();
                    Console.WriteLine("Product!");
                    var prod = o.Aggregate((x, acc) => x * acc);
                    return EnumerableEx.Return(new { sum, prod });
                }, 5)
                .Write();
        }

        private void ZipExample()
        {
            new[] { "A", "B", "C", "D" }
                .Zip(Odds(), (letter, odd) => letter + odd)
                .Write();
        }

        private void ZipExample2()
        {
            var classNames = new[] { "odd", "even" };
            new[] { "A", "B", "C", "D" }
                .Zip(classNames.Repeat(), (l, i) => l + i)
                .Write();
        }

        private void PairwiseExample()
        {
            Odds().Take(7)
                .Pairwise((l, r) => new { l, r })
                .Write();
        }

        // s.Prune(f) = f(s.Share())
        private void PruneExample()
        {
            Odds().Take(7)
                .Log()
                .Prune(n => n.Zip(n, (l, r) => new { l, r }))
                .Write();
        }

        private void ScanExample()
        {
            var nums = new[] { "one", "two", "three", "four" };
            nums.Log()
                .Publish(n => {
                    var lengths = n.Scan(0, (acc, curr) => acc + curr.Length);
                    var first = n.Scan("", (acc, x) => acc + x[0]);
                    return EnumerableEx.Concat(lengths.Select(i => i.ToString()), first);
                })
                .Write();
        }

        private void Scan0Example()
        {
            var nums = new[] { "one", "two", "three", "four" };
            nums.Log()
                .Scan0(0, (acc, curr) => acc + curr.Length)
                .Write();
        }

        private void SumEnumerableExample()
        {
            var s = new List<int> { 1, 2, 3, 4 };
            var sum = s.SumEnumerable();
            s.Add(7);
            sum.Write();

            s.Add(13);
            sum.Write();
        }

        private void UsingExample()
        {
            var d = EnumerableEx.Using(
                () => Disposable.Create(() => Console.WriteLine("Disposed!")),
                bd => Enumerable.Range(0, 5).Log());

            var r = from x in d
                    where x % 2 == 0
                    select new { x };

            r.Write();
            r.Write();
        }

        private void CatchExample()
        {
            Console.WriteLine("Without errors...");
            EnumerableEx.Catch(WithoutError("Catch")).Write();
            EnumerableEx.OnErrorResumeNext(WithoutError("OERN")).Write();

            Console.WriteLine();
            Console.WriteLine("With errors...");
            EnumerableEx.Catch(WithError("Catch")).Write();
            EnumerableEx.OnErrorResumeNext(WithError("OERN")).Write();
        }

        private IEnumerable<IEnumerable<string>> WithoutError(string message)
        {
            yield return EnumerableEx.Concat(
                EnumerableEx.Return(message + " 1"),
                EnumerableEx.Return(message + " 2"));

            yield return EnumerableEx.Return(message + " 3");
        }

        private IEnumerable<IEnumerable<string>> WithError(string message)
        {
            yield return EnumerableEx.Concat(
                EnumerableEx.Return(message + " 1"),
                EnumerableEx.Throw<string>(new Exception(message + " 2")));
            yield return EnumerableEx.Return(message + " 3");
            yield return EnumerableEx.Return(message + " 4");
        }

        private void LeftOuterJoinExample()
        {
            var odds = Odds().Take(10);
            var threes = new int?[] { 3, 6, 9, 12, 3 };

            var joined = from o in odds
                         join t in threes
                           on o equals t into j
                         from t2 in j.DefaultIfEmpty()
                         select new { o, t2 };

            joined.Write();
        }
    }
}