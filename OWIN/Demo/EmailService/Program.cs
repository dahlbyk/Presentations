using System;
using System.Diagnostics;
using Topshelf;

namespace EmailService
{
    class Program
    {
        static int Main(string[] args)
        {
            return (int)HostFactory.Run(
                host =>
                {
                    host.SetServiceName("EmailSender");
                    host.SetDescription("Sends email from queue.");
                    host.RunAsNetworkService();

                    host.Service<EmailService>();

                    if (Environment.UserInteractive)
                        Process.Start(EmailService.Url); // Spawn browser
                });
        }
    }
}
