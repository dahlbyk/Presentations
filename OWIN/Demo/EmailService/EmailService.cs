using System;
using System.Reactive.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace EmailService
{
    class EmailService : ServiceControl
    {
        private IDisposable app;
        public static readonly string Url = "http://localhost:23456";

        public bool Start(HostControl hostControl)
        {

            Observable.Interval(TimeSpan.FromSeconds(10)).Subscribe(_ => Queue.SendAll());

            app = WebApp.Start<Pipeline>(Url);

            Queue.Enqueued.Subscribe(
                next => Console.WriteLine("{0:hh:mm:ss} Sending '{1}'", DateTime.Now, next.Subject));

            var context = GlobalHost.ConnectionManager.GetHubContext<EmailHub>();
            Queue.Sent.Subscribe(
                next =>
                    {
                        context.Clients.All.emailSent(next);
                        Console.WriteLine("{0:hh:mm:ss} Sent '{1}'", DateTime.Now, next.Subject);
                    });
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            if (app != null)
                app.Dispose();

            return true;
        }
    }
}