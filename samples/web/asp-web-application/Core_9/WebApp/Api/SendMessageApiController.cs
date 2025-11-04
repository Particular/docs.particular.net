using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api;

[ApiController]
[Route("api/[controller]")]
public class SendMessageApiController : ControllerBase
{
    IMessageSession messageSession;

    #region MessageSessionInjection
    public SendMessageApiController(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }
    #endregion


    #region MessageSessionUsage
    [HttpGet]
    public async Task<string> Get()
    {
        var message = new Command();
        await messageSession.Send(message);
        return "Message sent to endpoint - refresh to send another";
    }
    #endregion
}