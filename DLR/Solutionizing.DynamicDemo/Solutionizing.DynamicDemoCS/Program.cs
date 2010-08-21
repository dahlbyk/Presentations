using System;
using System.Dynamic;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemoCS
{
    static class Program
    {
        static void Main(string[] args)
        {


            Console.ReadKey();
        }

        #region Simple Discount

        private static string some_python =
@"
def isValid(order):
    return order.ItemCount == 2
";

        private static object discount_params = new
        {
            Id = 1,
            Code = "TWO",
            ExpirationDate = new DateTime(2012, 1, 1),
            ValidationScript = some_python,
        };

        #endregion

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
                new ExpandoObject()
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

        #region DiscountDemo

        static void DiscountDemo()
        {
            var xml = XDocument.Load("discounts.xml");
            var repo = new DiscountRepository(xml);

            foreach (var discount in repo.GetAll())
                discount.Dump();
        }

        private static void Dump(this Discount discount)
        {
            Console.WriteLine("Discount {0}:", discount);
            foreach (var order in orders)
                Console.WriteLine("Valid for order {0}?\t{1}", order, discount.IsValid(order) ? "Yes" : "No");
            Console.WriteLine();
        }

        private static Order[] orders = new[] {
            new Order { ItemCount = 2, TotalAmount = 15.0m },
            new Order { ItemCount = 7, TotalAmount = 7.0m },
            new Order { ItemCount = 10, TotalAmount = 2.0m },
        };

        #endregion
    }
}
