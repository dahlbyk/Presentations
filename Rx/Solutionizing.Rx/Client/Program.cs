using System;
using MassTransit;
using MassTransit.Transports.Msmq;
using StructureMap;
using Core;
using System.Linq;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            MsmqEndpointConfigurator.Defaults(x =>
            {
                x.CreateMissingQueues = true;
            });

            ObjectFactory.Configure(x =>
            {
                x.AddRegistry(new ClientRegistry());
            });

            var bus = ObjectFactory.GetInstance<IServiceBus>();

            //bus.Subscribe<MyResponse>(m =>
            //{
            //    //Console.WriteLine("Response: " + m.ResponseText);
            //});

            var endPoint = ObjectFactory.GetInstance<IEndpointFactory>().GetEndpoint("msmq://localhost/test_server");


            while (true)
            {
                Console.Write("Text: ");
                var text = Console.ReadLine();

                Console.Write("Value: ");
                var value = Convert.ToInt32(Console.ReadLine());

                endPoint.Send(new MyCommand
                {
                    Text = text,
                    Value = value
                });
            }

            //Console.WriteLine("Sending...");
            //foreach (var i in Enumerable.Range(0, 10))
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine("Sending {0}", i);
            //    endPoint.Send(new MyCommand
            //    {
            //        Text = Guid.NewGuid().ToString(),
            //        Value = i
            //    }, x => x.SendResponseTo(bus));
            //    if (i % 2 == 0)
            //        endPoint.Send(new MyCommand
            //        {
            //            Text = "Dru",
            //            Value = i + 40
            //        }, x => x.SendResponseTo(bus));
            //}

            Console.WriteLine("Pausing...");

            Console.ReadKey();
        }
    }
}
