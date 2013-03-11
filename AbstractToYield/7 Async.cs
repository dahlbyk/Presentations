using System.Threading;
using System.Threading.Tasks;

namespace AbstractToYield
{
    class Async
    {
        async void Demo()
        {
            "Waiting...".Dump();
            Task<string> task = WaitSynch();

            for (int i = 0; i < 12; i++)
            {
                i.Dump();
                Thread.Sleep(500);
            }

            var result = await task;
            result.Dump();
        }

        public async Task<string> WaitAsynch()
        {
            await Task.Delay(5000);
            "Finished".Dump();
            return "Returned";
        }

        public async Task<string> WaitSynch()
        {
            Thread.Sleep(5000);
            "Finished".Dump();
            return "Returned";
        }
    }
}
