namespace Core.Handlers.DataAccess;

using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

public class API : IHandleMessages<MyMessage>
{
    #region BusinessData-SynchronizedStorageSession

    public Task Handle(MyMessage message,
        IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession
            .MyPersistenceSession();

        //Business logic

        return Task.CompletedTask;
    }

    #endregion
}

public static class MyOrmExtensions
{
    public static object MyPersistenceSession(this ISynchronizedStorageSession s)
    {
        throw new NotImplementedException();
    }
}