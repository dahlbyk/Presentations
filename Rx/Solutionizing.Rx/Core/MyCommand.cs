using System;

namespace Core
{
    public class MyCommand
    {
        public DateTime Sent
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public Guid CorrelationId
        {
            get;
            set;
        }
        public MyCommand()
        {
            CorrelationId = Guid.NewGuid();
            Sent = DateTime.Now;
        }
    }
}
