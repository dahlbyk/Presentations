using System;

namespace EmailService
{
    public class Email
    {
        public string Subject { get; set; }
        public DateTime? Queued { get; set; }
        public DateTime? Sent { get; set; }
    }
}