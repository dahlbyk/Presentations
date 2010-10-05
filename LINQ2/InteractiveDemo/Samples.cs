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

        private void DeferExample()
        {
            var message = "immediate!";
            var enumerableFactory = new Func<IEnumerable<string>>(() => EnumerableEx.Return(message));

            var m = enumerableFactory();
            var d = EnumerableEx.Defer(enumerableFactory);

            message = "deferred!";

            m.Run(x => Console.WriteLine("Immediate? {0}", x));
            d.Run(x => Console.WriteLine("Deferred? {0}", x));
        }

        private void ZipExample()
        {
            new[] { "A", "B", "C", "D" }
                .Zip(Odds().Take(3), (l, i) => l + i)
                .Run(Console.WriteLine);
        }

        private void PruneExample()
        {
            Odds().Take(7)
                .Prune(n => n.Zip(n, (l, r) => new { l, r }))
                .Run(Console.WriteLine);
        }

        private void ScanExample()
        {
            Odds().Take(7)
                .Do(Console.WriteLine)
                .Scan(10, (curr, acc) => curr * acc)
                .Run(Console.WriteLine);
        }

        private void Scan0Example()
        {
            Odds().Take(7)
                .Do(Console.WriteLine)
                .Scan0(10, (curr, acc) => curr * acc)
                .Run(Console.WriteLine);
        }

        private void SumEnumerableExample()
        {
            var s = new List<int> { 1, 2, 3, 4 };
            var sum = s.SumEnumerable();
            s.Add(7);
            Console.WriteLine(sum.Single());
        }

        private void UsingExample()
        {
            var d = EnumerableEx.Using(
                () => Disposable.Create(() => Console.WriteLine("Disposed!")),
                bd => Enumerable.Range(0, 5).Do(Console.WriteLine));

            d.Where(x => x % 2 == 0).Run(Console.WriteLine);
        }

        private void CatchExample()
        {
            Console.WriteLine("Without errors...");
            EnumerableEx.Catch(WithoutError("Catch")).Run(Console.WriteLine);
            EnumerableEx.OnErrorResumeNext(WithoutError("OERN")).Run(Console.WriteLine);

            Console.WriteLine();
            Console.WriteLine("With errors...");
            EnumerableEx.Catch(WithError("Catch")).Run(Console.WriteLine);
            EnumerableEx.OnErrorResumeNext(WithError("OERN")).Run(Console.WriteLine);
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
        }
    }
}