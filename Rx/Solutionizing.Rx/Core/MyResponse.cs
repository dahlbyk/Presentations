using System;

namespace Core
{
    public class MyResponse
    {
        public MyResponse()
        {
            Sent = DateTimeOffset.Now;
        }
        public DateTimeOffset Sent
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
        public string ResponseText
        {
            get;
            set;
        }
    }
}
