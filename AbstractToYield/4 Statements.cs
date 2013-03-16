using System;
using System.Collections;
using System.Threading;

namespace AbstractToYield
{
    class Statements
    {
        /*  Selection
         *
         *  if ... else
         *  switch
         */

        /*  Iteration
         *
         *  while
         *  do ... while
         *  for
         *  foreach
         */

        /*  Jump
         *
         *  break
         *  continue
         *  goto
         *  return
         */

        /*  Exception Handling
         *
         *  throw
         *  try ... catch ... finally
         */

        /*  Miscellaneous
         * 
         *  checked { ... }
         *  unchecked { ... }
         *
         *  lock (...) { ... }
         *  using (...) { ... }
         */

        void Test()
        {
            using (new DumpingDisposable("BOOM"))
                do
                {
                    switch (DateTime.Now.Ticks % 6)
                    {
                        case 0:
                        case 2:
                        case 4:
                            {
                                var s = "even";
                                s.Dump();
                                continue;
                            }
                        case 5:
                            "FIVE! It's quitting time.".Dump();
                            return;
                        case 3:
                            "three".Dump();
                            break;
                        default:
                            {
                                var s = "neither";
                                s.Dump();
                                goto skipCount;
                            }
                    }
                    simpleCount.Dump();

                skipCount:
                    "...".Dump();

                } while (TestShouldContinue());
        }

        private int simpleCount;

        private bool TestShouldContinue()
        {
            "Continue?".Dump();
            Thread.Sleep(300);
            return simpleCount++ < 5;
        }

        void Each()
        {
            var notEnumerable = new NotIEnumerable();

            foreach (var x in notEnumerable)
                x.Dump();
        }
    }

    class NotIEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return "first";
            yield return 2;
        }
    }

    class DumpingDisposable : IDisposable
    {
        private readonly string message;

        public DumpingDisposable(string message)
        {
            this.message = message;
        }

        public void Dispose()
        {
            "Disposing...".Dump();
            message.Dump();
        }
    }
}
