using System;
using System.Diagnostics;
using Microsoft.Owin.Hosting;

namespace Demo
{
    public class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:12345";
            using (WebApp.Start<Startup>(url))
            {
                Process.Start(url); // Spawn browser

                Console.ReadLine();
            }
        }
    }
}
