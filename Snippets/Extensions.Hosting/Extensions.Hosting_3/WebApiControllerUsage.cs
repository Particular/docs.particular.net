using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

public class WebApiControllerUsage
{
    #region web-api-usage
    [ApiController]
    class WebApiController : ControllerBase
    {
        IMessageSession messageSession;

        public WebApiController(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            await messageSession.Send(new MessageFromWebApi());
            return Ok("message was sent successfully");
        }
    }
    #endregion

    class MessageFromWebApi
    {
    }
}