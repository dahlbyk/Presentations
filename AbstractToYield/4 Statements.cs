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
         *  fixed (...) { ... }         // unsafe
         *  lock (...) { ... }
         */

        void Test()
        {
            do
            {
                switch (DateTime.Now.Second % 6)
                {
                    case 0:
                    case 2:
                    case 4:
                        {
                            var s = "even";
                            s.Dump();
                            // continue;
                        }
                        break;
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
            Thread.Sleep(1200);
            return simpleCount++ < 5;
        }

        void Four()
        {
            var words = new[] { "one", "two", "three" };

            for (var i = 0; i < words.Length; i++)
                words[i].Dump();
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
}
