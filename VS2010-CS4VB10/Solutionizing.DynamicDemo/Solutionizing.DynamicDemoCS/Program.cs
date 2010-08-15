using System;
using System.Dynamic;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Solutionizing.DynamicDemoCS
{
    static class Program
    {
        static void Main(string[] args)
        {
            DiscountDemo();

            Console.ReadKey();
        }

        public static void OptionalTest(string name, int? age = null, bool hasPants = true, bool hasShirt = false)
        {
            var m = MethodInfo.GetCurrentMethod();

            var p = m.GetParameters();
        }

        public static IEnumerable<object> Test()
        {
            yield return new { test = "hi" };
        }

        #region RandomTypeDemo

        public static dynamic GetRandom()
        {
            if (DateTime.Now.Second % 2 == 0)
                return "hello";
            else
                return new[] { "hello", "goodbye" };
        }

        static void RandomTypeDemo()
        {
            foreach (var i in System.Linq.Enumerable.Range(0, 16))
            {
                dynamic x = GetDynamicX(i);
                dynamic y = GetDynamicY(i);

                dynamic z = null;
                try
                {
                    z = x + y;
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
            dynamic ex = new ExpandoObject();

            ex.Name = "Keith";
            ex.Age = "pi";

            Console.WriteLine(new { ex.Name, ex.Age, ex.ShirtColor });

            ex.Increment = new Func<int, int>(i => i + 1);
            Console.WriteLine("1 + 1 = {0}", ex.Increment(1));
        }

        #endregion

        #region DiscountDemo

        static void DiscountDemo()
        {
            var xml = XDocument.Load("discounts.xml");
            var repo = new DiscountRepository(xml);

            var orders = new[] {
                new Order { ItemCount = 2, TotalAmount = 15.0m },
                new Order { ItemCount = 7, TotalAmount = 7.0m },
                new Order { ItemCount = 10, TotalAmount = 2.0m }
            };

            foreach (var order in orders)
            {
                Console.WriteLine("Order: {0}", order);
                foreach (var discount in repo.GetAll())
                {
                    Console.WriteLine("Discount {0} valid?\t{1}!", discount.Code, discount.IsValid(order) ? "Yes" : "No");
                }
                Console.WriteLine();
            }
        }

        #endregion
    }
}
