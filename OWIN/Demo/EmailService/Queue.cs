using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using System.Threading;

namespace EmailService
{
    static class Queue
    {
        private static readonly ConcurrentQueue<Email> messages = new ConcurrentQueue<Email>();
        private static readonly Subject<Email> enqueued = new Subject<Email>();
        private static readonly Subject<Email> sent = new Subject<Email>();
        private static readonly Random random = new Random();

        public static void Enqueue(Email message)
        {
            message.Queued = DateTime.Now;
            messages.Enqueue(message);
            enqueued.OnNext(message);
        }

        public static void SendAll()
        {
            Email message;
            while(messages.TryDequeue(out message))
            {
                Thread.Sleep(random.Next(100, 1000));
                message.Sent = DateTime.Now;
                sent.OnNext(message);
            }
        }

        public static IObservable<Email> Enqueued { get { return enqueued; } }
        public static IObservable<Email> Sent { get { return sent; } }
    }
}