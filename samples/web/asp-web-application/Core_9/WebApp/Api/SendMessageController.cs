using Microsoft.AspNetCore.Mvc;
using NServiceBus;

[ApiController]
[Route("api/[controller]")]
public class SendMessageController : ControllerBase
{
    IMessageSession messageSession;

    #region MessageSessionInjection
    public SendMessageController(IMessageSession messageSession)
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
        return "Message sent to endpoint";
    }
    #endregion
}