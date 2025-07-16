using Microsoft.AspNetCore.Mvc;
using NServiceBus;

[ApiController]
[Route("")]
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
        var message = new MyMessage();
        await messageSession.Send(message);
        return "Message sent to endpoint";
    }
    #endregion
}