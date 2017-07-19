using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler-sqlPersistenceSession
public class HandlerThatUsesSession :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<HandlerThatUsesSession>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var sqlPersistenceSession = context.SynchronizedStorageSession.SqlPersistenceSession();
        log.Info(sqlPersistenceSession.Connection.ConnectionString);
        // use Connection and/or Transaction of ISqlStorageSession to persist or query the database
        return Task.CompletedTask;
    }
}
#endregion