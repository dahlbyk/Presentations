using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Solutionizing.DynamicDemoCS
{
    static class Program
    {
        static void Main(string[] args)
        {
            dynamic ex = new MyExpandoObject();

            ex.Name = "Keith";
            ex.BirthDate = new DateTime(2008, 1, 1);

            ex.ToString2 = new Func<string>(() => ex.Name + " " + ex.BirthDate);


            Console.WriteLine(ex.ToString2());

            Console.ReadKey();
        }

        public static dynamic GetRandom()
        {
            if (DateTime.Now.Second % 2 == 0)
                return "hello";
            else
                return new[] { "hello", "goodbye" };
        }
        


        #region RandomTypeDemo

        static void RandomTypeDemo()
        {
            foreach (var i in System.Linq.Enumerable.Range(0, 16))
            {
                dynamic x = GetDynamicX(i);
                dynamic y = GetDynamicY(i);

                dynamic z = null;
                try
                {
                    z = x + y + 5;
                }
                catch (Exception ex)
                {
                    z = ex.Message;
                }

                Console.WriteLine("{0}\n    {1} + {2} = {3}", z.GetType(), x, y, z);
                if ('q' == Console.ReadKey().KeyChar)
                    break;
            }
        }

        static dynamic GetDynamicX(int i)
        {
            switch (i % 4)
            {
                case 0: return 1;
                case 2: return "XX";
                case 1: return 1.5;
                default: return DateTime.Now;
            }
        }

        static dynamic GetDynamicY(int i)
        {
            switch (i / 4)
            {
                case 0: return 2;
                case 1: return 3.14m;
                case 2: return new TimeSpan(12, 0, 0);
                default: return "YY";
            }
        }

        #endregion

        #region ExpandoDemo

        public static void ExpandoDemo()
        {
            var expandos = new dynamic[] {
                new ExpandoObject(),
                new MyExpandoObject()
            };

            foreach (var ex in expandos)
            {
                ex.Name = "Keith";

                Console.WriteLine(ex.Name);

                ex.Increment = new Func<int, int>(i => i + 1);
                Console.WriteLine("1 + 1 = {0}", ex.Increment(1));
            }
        }

        #endregion
    }
}
