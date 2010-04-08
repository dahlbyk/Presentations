using System;

namespace AtoV.Loggers
{
    public class ConsoleLogger : AbstractLogger
    {
        public static readonly ConsoleLogger Instance = new ConsoleLogger();

        public override void Log(string msg)
        {
            Console.WriteLine("** "+msg);
        }

        public override void Log(string msg, string category)
        {
            Console.WriteLine("!!!!!!!");
            base.Log(msg, category);
            Console.WriteLine("!!!!!!!");
        }
    }

    public class DisposableConsoleLogger : ConsoleLogger, IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Disposing!");
        }
    }
}
