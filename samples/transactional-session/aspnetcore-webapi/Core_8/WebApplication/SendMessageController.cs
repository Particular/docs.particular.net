using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using NServiceBus.TransactionalSession;

[ApiController]
[Route("")]
public class SendMessageController : Controller
{
    ITransactionalSession messageSession;

    #region MessageSessionInjection
    public SendMessageController(ITransactionalSession messageSession)
    {
        this.messageSession = messageSession;
    }
    #endregion


    #region MessageSessionUsage
    [HttpGet]
    public async Task<string> Get()
    {
        var message = new MyMessage();
        await messageSession.SendLocal(message)
            .ConfigureAwait(false);

        return "Message sent to endpoint";
    }
    #endregion
}
