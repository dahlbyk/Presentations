using System;
using System.Dynamic;
using System.Xml.Linq;
using Solutionizing.DynamicDemo.Data;
using System.Linq;

namespace Solutionizing.DynamicDemo
{
    static class Program
    {
        #region RandomTypeDemo

        public static void RandomTypeDemo1()
        {
            foreach (var i in Enumerable.Range(0, 3))
            {
                dynamic r = GetRandom(i);
                Console.WriteLine(r.Length);
            }
        }

        public static dynamic GetRandom(int n)
        {
            switch (n % 3)
            {
                case 0: return "hello";
                case 1: return new[] { "hello", "goodbye" };
                default: return 3;
            }
        }

        public static void RandomTypeDemo2()
        {
            foreach (var i in Enumerable.Range(0, 16))
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

            Console.WriteLine(ex.Name);

            ex.Increment = new Func<int, int>(i => i + 1);
            Console.WriteLine("1 + 1 = {0}", ex.Increment(1));
        }

        #endregion

        #region DiscountDemo

        public static void SingleDiscountDemo()
        {
            var some_python =
@"
def isValid(order):
    return order.ItemCount == 2
";

            object discount_params = new
            {
                Id = 1,
                Code = "TWO",
                ExpirationDate = new DateTime(2012, 1, 1),
                ValidationScript = some_python,
            };

            var discount = new Discount(discount_params);

            discount.Dump();
        }

        public static void DiscountDemo()
        {
            var repo = GetRepo();

            foreach (var discount in repo.GetAll())
                discount.Dump();
        }

        private static IDiscountRepository GetRepo()
        {
            return new XmlDiscountRepository(LoadXml());
            //return new DXmlDiscountRepository(LoadXml());
            //return new WebMatrixDataDiscountRepository();
            //return new MassiveDiscountRepository();
        }

        private static void Dump(this Discount discount)
        {
            Console.WriteLine("Discount {0}:", discount.Code);
            foreach (var order in orders)
                Console.WriteLine("Valid for order {0}?\t{1}", order, discount.IsValid(order) ? "Yes" : "No");
            Console.WriteLine();
        }

        private static Order[] orders = new[] {
            new Order { ItemCount = 2, TotalAmount = 15.0m },
            new Order { ItemCount = 7, TotalAmount = 7.0m },
            new Order { ItemCount = 10, TotalAmount = 2.0m },
        };


        public static void WriteDiscount()
        {
            var writer = new WebMatrixDataDiscountRepository();

            writer.Save(new
            {
                Code = "SEVEN",
                ValidationScript = "def isValid(order): return order.ItemCount == 7",
                ExpirationDate = new DateTime(2012, 12, 12),
            });
        }

        private static XDocument LoadXml()
        {
            return XDocument.Load("discounts.xml");
        }

        #endregion
    }
}
