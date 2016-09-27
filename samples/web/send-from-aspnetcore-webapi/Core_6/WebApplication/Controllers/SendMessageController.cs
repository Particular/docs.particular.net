using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class SendMessageController : Controller
    {
        private readonly IMessageSession _messageSession;

        #region MessageSessionInjection
        public SendMessageController(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }
        #endregion


        #region MessageSessionUsage
        [HttpGet]
        public async Task<string> Get()
        {
            await _messageSession.Send(new MyMessage());
            return "Message sent to endpoint";
        }
        #endregion
    }
}
