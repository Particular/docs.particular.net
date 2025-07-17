using System;
using System.Threading.Tasks;
using NServiceBus.TransactionalSession;

public class ServiceUsingTransactionalSession(MyDataContext dataContext, ITransactionalSession messageSession)
{
    public async Task<string> Execute()
    {
        var id = Guid.NewGuid().ToString();

        await dataContext.MyEntities.AddAsync(new MyEntity { Id = id, Processed = false });

        var message = new MyMessage { EntityId = id };
        await messageSession.Send(message);

        return id;
    }
}