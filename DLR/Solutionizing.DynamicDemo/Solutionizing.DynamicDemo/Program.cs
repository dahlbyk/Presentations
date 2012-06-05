using System;
using System.Dynamic;
using System.Xml.Linq;
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;
using Solutionizing.DynamicDemo.Data;

namespace Solutionizing.DynamicDemo
{
    static class Program
    {
        #region Sandbox

        public static void Sandbox()
        {

        }

        #endregion

        #region Lengths

        static readonly dynamic[] stuffWithLengths = new dynamic[]
        {
            "hello",
            new[] { "hello", "goodbye" },
            new { Length = "long" },
            3,
        };

        public static void GetLengths()
        {
            foreach (dynamic r in stuffWithLengths)
            {
                Console.WriteLine(r.Length);
            }
        }

        #endregion

        #region Operators

        static readonly dynamic[] xValues = new dynamic[]
        {
            1,
            "XX",
            1.5,
            DateTime.Now,
        };

        static readonly dynamic[] yValues = new dynamic[]
        {
            2,
            3.14m,
            new TimeSpan(12, 0, 0),
            "YY",
        };

        static void TryAdd(dynamic x, dynamic y)
        {
            try
            {
                dynamic z = x + y;
                Console.WriteLine("{0} + {1} = {2} ({3})", x, y, z, z.GetType());
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} + {1} = ERROR:\n{2}", x, y, ex.Message);
            }
        }

        public static void Add()
        {
            foreach (var x in xValues)
                foreach (var y in yValues)
                    TryAdd(x, y);
        }

        #endregion

        #region Building Dynamic Objects

        public static void Expando()
        {
            dynamic ex = new ExpandoObject();

            ex.Name = "Keith";

            Console.WriteLine(ex.Name);

            ex.Increment = new Func<int, int>(i => i + 1);
            Console.WriteLine("1 + 1 = {0}", ex.Increment(1));
        }

        public static void ImpromptuBuilder()
        {
            dynamic New = Builder.New();

            dynamic person = New.Person(
                FirstName: "Robert",
                LastName: "Paulson"
            )
            .Age(42)
            .Sing(ReturnVoid.Arguments(() => Console.WriteLine("TROOOOOOOOGDOOOOOOOOOOOR!")))
            .Greet(Return<string>.ThisAndArguments(
                (@this, greeting) => string.Format("{0} {1} {2}", greeting, @this.FirstName, @this.LastName)));

            Console.WriteLine(person.FirstName);
            Console.WriteLine(person.LastName);
            Console.WriteLine(person.Age);
            Console.WriteLine(person.Greet("His name is"));
            person.Sing();

            // http://code.google.com/p/impromptu-interface/wiki/UsageBuilder
        }

        #endregion

        #region Currying Too

        static readonly Action<string, int, string> logger = (file, lineNumber, error) =>
            Console.WriteLine("Error in {0}, line {1}: {2}", file, lineNumber, error);

        static string PostUrl(string author, DateTime date, string slug)
        {
            return string.Format(
                "http://lostechies.com/{0}/{1:yyyy}/{1:MM}/{1:dd}/{2}",
                author, date, slug);
        }

        static void Static()
        {
            Func<string, Func<int, Action<string>>> curried =
                f => l => e => logger(f, l, e);

            var errorInProgram = curried("Program.cs");
            errorInProgram(12)("Invalid syntax");
            errorInProgram(16)("Missing return statement");
        }

        static void Dynamic()
        {
            var curried = Impromptu.Curry(logger);

            var errorInProgram = curried("Program.cs");
            errorInProgram(12)("Invalid syntax");
            errorInProgram(16, "Missing return statement");
        }

        #endregion

        #region Currying

        static readonly Func<int, double, float, double> adder = (x, y, z) => x + y + z;

        public static void StaticCurrying()
        {
            Func<int, Func<double, Func<float, double>>> curried =
                x => y => z => adder(x, y, z);

            Console.WriteLine(curried(2)(3.14)(42.0f));

            Func<int, double, Func<float, double>> curried2 =
                (x, y) => z => adder(x, y, z);

            Console.WriteLine(curried2(2, 3.14)(42.0f));
        }

        public static void DynamicCurrying()
        {
            var curried = Impromptu.Curry(adder);

            Console.WriteLine(curried(1, 2, 3)); // bug!
            Console.WriteLine(curried(1, 2)(3));
            Console.WriteLine(curried(1)(2, 3));
            Console.WriteLine(curried(1)(2)(3));

            var partiallyApplied = curried(2, 3.14);

            Console.WriteLine(partiallyApplied(42.0f));
            Console.WriteLine(partiallyApplied(10));
        }

        #endregion

        #region Discounts

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
                ValidationScriptType = "text/python",
                ValidationScript = some_python,
            };

            var discount = new Discount(discount_params);

            discount.Dump();
        }

        public static void XmlDiscounts()
        {
            var repo = new XmlDiscountRepository(xml);

            repo.DumpAll();
        }

        public static void DXmlDiscounts()
        {
            var repo = new DXmlDiscountRepository(xml);

            repo.DumpAll();
        }

        public static void WebMatrixDataDiscounts()
        {
            var repo = new WebMatrixDataDiscountRepository();

            repo.DumpAll();
        }

        public static void MassiveDiscounts()
        {
            var repo = new MassiveDiscountRepository();

            repo.DumpAll();
        }

        public static void DapperDiscounts()
        {
            var repo = new DapperDiscountRepository();

            repo.DumpAll();
        }

        #endregion

        #region Write

        public static void WriteDiscount()
        {
            var writer = new WebMatrixDataDiscountRepository();

            writer.Save(new
            {
                Code = "SEVEN",
                ValidationScript = "def isValid(order): return order.ItemCount == 7",
                ValidationScriptType = "text/python",
                ExpirationDate = new DateTime(2012, 12, 12),
            });
        }

        #endregion

        private static readonly XDocument xml = XDocument.Load("discounts.xml");
    }
}
