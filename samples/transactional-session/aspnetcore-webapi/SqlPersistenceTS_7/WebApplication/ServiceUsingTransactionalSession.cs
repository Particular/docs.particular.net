using System;
using System.Threading.Tasks;
using NServiceBus.TransactionalSession;

public class ServiceUsingTransactionalSession
{
    private readonly MyDataContext dataContext;
    private readonly ITransactionalSession messageSession;

    public ServiceUsingTransactionalSession(MyDataContext dataContext, ITransactionalSession messageSession)
    {
        this.dataContext = dataContext;
        this.messageSession = messageSession;
    }

    public async Task<string> Execute()
    {
        var id = Guid.NewGuid().ToString();

        await dataContext.MyEntities.AddAsync(new MyEntity { Id = id, Processed = false });

        var message = new MyMessage { EntityId = id };
        await messageSession.SendLocal(message);

        return id;
    }
}