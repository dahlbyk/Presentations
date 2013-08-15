using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AbstractToYield
{
    class Async
    {
        void Demo2()
        {
            Demo().Result.Dump();
        }

        async Task<string> Demo()
        {
            "Waiting...".Dump();
            Task<string> task = WaitAsynch();

            for (int i = 0; i < 12; i++)
            {
                i.Dump();
                Thread.Sleep(250);
            }

            return await task;
        }

        public async Task<string> WaitSynch()
        {
            Thread.Sleep(3000);
            "Finished".Dump();
            return "Returned";
        }

        public async Task<string> WaitAsynch()
        {
            await Task.Delay(3000);
            "Finished".Dump();
            return "Returned";
        }

        public async Task<string> WaitTimeSpan(int seconds = 1)
        {
            await TimeSpan.FromSeconds(seconds);
            "Finished".Dump();
            return "Returned";
        }
    }

    // http://blogs.msdn.com/b/pfxteam/archive/2011/01/13/10115642.aspx
    static class AsyncExtensions
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timespan)
        {
            return Task.Delay(timespan).GetAwaiter();
        }
    }
}
