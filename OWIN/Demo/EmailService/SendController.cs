using System.Web.Http;

namespace EmailService
{
    public class SendController : ApiController
    {
        public void Post([FromBody]Email message)
        {
            Queue.Enqueue(message);
        }
    }
}