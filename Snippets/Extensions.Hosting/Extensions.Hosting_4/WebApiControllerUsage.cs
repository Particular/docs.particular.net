using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

public class WebApiControllerUsage
{
    #region web-api-usage
    [ApiController]
    class WebApiController(IMessageSession messageSession) : ControllerBase
    {
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