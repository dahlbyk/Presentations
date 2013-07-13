using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtoV.Loggers;

namespace AtoV
{
    static class Program
    {
        static void Main(string[] args)
        {
        //    ValRefFun();
        //    Members();
            //Logging();
            DisposeFUn();
        }

        static void ValRefFun()
        {
            var value1 = new MyValue("Value String", new DateTime(2010, 1, 1));
            var value2 = default(MyValue);
            value2.MyString = "Value String 2";
            value2.MyDate = DateTime.MinValue;

            Console.WriteLine("Before: " + new { value1.MyDate, value1.MyString });
            PlayWithValue(out value1);
            Console.WriteLine("After:  " + new { value1.MyDate, value1.MyString });


            var Class1 = new MyClass("Class String", new DateTime(2010, 1, 1));

            Console.WriteLine("Before: " + new { Class1.MyDate, MyString = Class1.myString });
            PlayWithClass(Class1);
            Console.WriteLine("After:  " + new { Class1.MyDate, MyString = Class1.myString });
        }

        static void PlayWithValue(out MyValue value)
        {
            //value = new MyValue("yay out", DateTime.MaxValue);
            value.MyString = "Changed MyString!";
            value.MyDate = DateTime.Now.AddDays(2);
            Console.WriteLine(new { value.MyDate, value.MyString });
        }
        static void PlayWithClass(MyClass value)
        {
            value.myString = "Changed MyString!";
            value.MyDate = DateTime.Now.AddDays(2);
            Console.WriteLine(new { value.MyDate, MyString = value.myString });
        }

        static void Members()
        {
            Console.WriteLine("Pre-constructation.");
            var c = new MyClass();

            Console.WriteLine(c.myString);
            Console.WriteLine(c.DoubleMyString);

            c.DoubleMyString = "alpha";
            Console.WriteLine(c.myString);
            c.DoubleMyString = "beta";
            Console.WriteLine(c.myString);

            MyClass.DOABetterDance();
        }

        private static void Logging()
        {
            var logger = ConsoleLogger.Instance;

            logger.LogFormat("cat", "{0} {1}", "Category {", 1+2);
        }

        private static void DisposeFUn()
        {
            using (var d = new DisposableConsoleLogger())
                d.Log("Hello, disposable stuff!");
        }
    }
}

