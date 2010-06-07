using System;
using System.Linq;
using Core;
using MassTransit;
using MassTransit.Reactive;
using MassTransit.Transports.Msmq;
using StructureMap;
using System.Threading.Tasks;
using System.Concurrency;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Program
    {
        class Attempt
        {
            public string UserName { get; set; }
            public string IP { get; set; }

            public override string ToString()
            {
                return new { UserName, IP }.ToString();
            }
        }

        static void Main(string[] args)
        {
            var users = new[] {
                new Attempt { UserName = "Dru", IP = "1" },
                new Attempt { UserName = "Keith", IP = "1" },
                new Attempt { UserName = "Keith", IP = "2" },
                new Attempt { UserName = "Keith", IP = "3" },
                new Attempt { UserName = "Dru", IP = "3" },
                new Attempt { UserName = "Tim", IP = "4" },
                new Attempt { UserName = "Dru", IP = "3" }
            }.Do(a => Thread.Sleep(300));


            var obs = users.ToObservable(Scheduler.ThreadPool);
                //.Do(n => Thread.Sleep(n * 1));

            var h4x0rz =
                from n in obs
                group n by n.UserName into g
                from b in g.BufferWithTime(TimeSpan.FromSeconds(1))
                where b.Count > 2
                select new
                {
                    UserName = g.Key,
                    IPs = string.Join(" ", b.Select(a => a.IP).ToArray())
                };

            h4x0rz.Subscribe(Console.WriteLine);

            //while (true)
            //{
            //    Console.Write(".");
            //    Thread.Sleep(500);
            //}

            Console.ReadKey();
        }

        static void MassTransit()
        {
            MsmqEndpointConfigurator.Defaults(x =>
            {
                x.CreateMissingQueues = true;
            });

            ObjectFactory.Configure(x =>
            {
                x.AddRegistry(new ServerRegistry());
            });

            var bus = ObjectFactory.GetInstance<IServiceBus>();

            // bus.Subscribe<MyCommand>(c => Console.WriteLine(new{ c.Sent, c.Text, c.Value }));

            var obs = bus.AsObservable<MyCommand>();

            obs.Select(c => new { c.Sent, c.Text, c.Value })
                .Subscribe(Console.WriteLine);

            Console.ReadKey();
        }
    }
}
