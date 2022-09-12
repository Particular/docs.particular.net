using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus.TransactionalSession;

[ApiController]
[Route("")]
public class SendMessageController : Controller
{
    readonly ITransactionalSession messageSession;
    readonly MyDataContext dataContext;

    public SendMessageController(ITransactionalSession messageSession, MyDataContext dataContext)
    {
        this.messageSession = messageSession;
        this.dataContext = dataContext;
    }

    #region txsession-controller
    [HttpGet]
    public async Task<string> Get()
    {
        var id = Guid.NewGuid().ToString();

        await dataContext.MyEntities.AddAsync(new MyEntity { Id = id, Processed = false });

        var message = new MyMessage { EntityId = id };
        await messageSession.SendLocal(message)
            .ConfigureAwait(false);

        return $"Message with entity ID '{id}' sent to endpoint";
    }
    #endregion
}
