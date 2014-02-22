using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AbstractToYield
{
    class Async
    {
        private static string Return(
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            return "Returned from {0}\n{1} @ line {2}".FormatWith(memberName, filePath, lineNumber);
        }

        async void Demo()
        {
            "Waiting...".Dump();
            Task<string> task = WaitSynch(2);

            for (int i = 0; i < 12; i++)
            {
                i.Dump();
                Thread.Sleep(250);
            }

            var result = await task;
            result.Dump();
        }

        public async Task<string> WaitSynch(int seconds = 1)
        {
            Thread.Sleep(1000 * seconds);
            "Finished".Dump();
            return Return();
        }

        public async Task<string> WaitAsynch(int seconds = 1)
        {
            await Task.Delay(1000 * seconds);
            "Finished".Dump();
            return Return();
        }

        public async Task<string> WaitTimeSpan(int seconds = 1)
        {
            await TimeSpan.FromSeconds(seconds);
            "Finished".Dump();
            return Return();
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
