using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NServiceBus.TransactionalSession;

[ApiController]
[Route("")]
public class SendMessageController : Controller
{
    readonly MyDataContext dataContext;

    public SendMessageController(MyDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    #region txsession-controller
    [HttpGet]
    public async Task<string> Get([FromServices] ITransactionalSession messageSession)
    {
        var id = Guid.NewGuid().ToString();

        await dataContext.MyEntities.AddAsync(new MyEntity { Id = id, Processed = false });

        var message = new MyMessage { EntityId = id };
        await messageSession.SendLocal(message);

        return $"Message with entity ID '{id}' sent to endpoint";
    }
    #endregion

    #region txsession-controller-query
    [HttpGet("/all")]
    public async Task<List<MyEntity>> GetAll()
    {
        return await dataContext.MyEntities.ToListAsync();
    }
    #endregion

    #region txsession-controller-attribute
    [HttpGet("/service")]
    [RequiresTransactionalSession]
    public async Task<string> Get([FromServices] ServiceUsingTransactionalSession service)
    {
        var id = await service.Execute();

        return $"Message with entity ID '{id}' sent to endpoint";
    }
    #endregion
}