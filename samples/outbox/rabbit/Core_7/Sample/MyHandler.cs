using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    #region Handler
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Processing MessageId {context.MessageId}");

        var sqlPersistenceSession = context.SynchronizedStorageSession.SqlPersistenceSession();

        using (var command = sqlPersistenceSession.Connection.CreateCommand())
        {
            command.CommandText = $"insert into BusinessObject (MessageId) values ('{context.MessageId}')";
            command.Transaction = sqlPersistenceSession.Transaction;
            await command.ExecuteNonQueryAsync();
        }
    }
    #endregion
}