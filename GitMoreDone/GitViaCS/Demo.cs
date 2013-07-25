using System;
using LibGit2Sharp;

namespace GitViaCS
{
    public static class Demo
    {
        static void New()
        {
            var newRepoPath = Repository.Init(@"C:\Temp\GitViaCS\{0:hhmmss}".FormatWith(DateTime.Now));

            using (var repo = new Repository(newRepoPath))
            {
                repo.Info.IsHeadOrphaned.Dump();
            }
        }

        static void Glimpse()
        {
            using (var repo = new Repository(@"C:\Dev\GitHub\Glimpse"))
            {
                Console.WriteLine();
                Console.WriteLine("=======================");
                Console.WriteLine("Refs");
                Console.WriteLine("=======================");
                repo.Refs.Dump();
            }
        }
    }
}
