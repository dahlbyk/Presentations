using System;
using System.Dynamic;
using System.Xml.Linq;
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;
using Solutionizing.DynamicDemo.Data;

namespace Solutionizing.DynamicDemo
{
    public static class Program
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

            #region Duck Typing

            //IPerson duckTypedPerson = Impromptu.ActLike<IPerson>(person);
            //duckTypedPerson.LastName = "Martin";

            //IGreet duckTypedGreeter = Impromptu.ActLike<IGreet>(person);
            //Console.WriteLine(duckTypedGreeter.Greet("Clean coder:"));

            #endregion
        }

        public interface IPerson
        {
            string FirstName { get; }
            string LastName { get; set; }
        }

        public interface IGreet
        {
            string Greet(string greeting);
        }

        #endregion

        #region Currying

        static readonly Action<string, int, string> logger = (file, lineNumber, error) =>
            Console.WriteLine("Error in {0}, line {1}: {2}", file, lineNumber, error);

        public static void Static()
        {
            Func<string, Func<int, Action<string>>> curried =
                f => l => e => logger(f, l, e);

            var errorInProgram = curried("Program.cs");
            errorInProgram(12)("Invalid syntax");
            errorInProgram(16)("Missing return statement");
        }

        public static void Dynamic()
        {
            var curried = Impromptu.Curry(logger);

            var errorInProgram = curried("Program.cs");
            errorInProgram(12)("Invalid syntax");
            errorInProgram(16, "Missing return statement");
        }

        #endregion

        #region Single Discount

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

        #endregion

        #region Xml Discounts

        private static readonly XDocument xml = XDocument.Load("discounts.xml");

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

        #endregion

        #region SQL Discounts

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

        public static void WriteWithWebMatrix()
        {
            var writer = new WebMatrixDataDiscountRepository();

            writer.Test("SEVEN", 7);
        }

        public static void WriteWithDapper()
        {
            var writer = new DapperDiscountRepository();

            writer.Test("TEN", 10);
        }

        #endregion
    }
}
