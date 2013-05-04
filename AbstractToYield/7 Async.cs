using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AbstractToYield
{
    class Async
    {
        async void Demo()
        {
            "Waiting...".Dump();
            Task<string> task = WaitAsynch();

            for (int i = 0; i < 12; i++)
            {
                i.Dump();
                Thread.Sleep(250);
            }

            var result = await task;
            result.Dump();
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

        public async Task<string> WaitTimeSpan()
        {
            await TimeSpan.FromMilliseconds(3000);
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
