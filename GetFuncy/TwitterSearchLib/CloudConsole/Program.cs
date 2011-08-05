using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = TwitterSearch.TwitterCloud.GetCloud("#stldodn", 10, 0.05, 0.9);
            foreach (var w in c.Cloud.Take(100))
                Console.WriteLine(w);

            Console.ReadKey();
        }
    }
}
