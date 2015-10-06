using System;
using Simple;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var ikfb = new FizzBuzz();

            for (var i = 1; i <= 100; i++)
            {
                Console.WriteLine(ikfb.Translate(i));
            }
        }
    }
}
