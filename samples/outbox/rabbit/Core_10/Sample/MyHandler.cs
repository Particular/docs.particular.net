using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    #region Handler
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Processing MessageId {context.MessageId}");

        var sqlPersistenceSession = context.SynchronizedStorageSession.SqlPersistenceSession();

        await using var command = sqlPersistenceSession.Connection.CreateCommand();
        command.CommandText = $"insert into BusinessObject (MessageId) values ('{context.MessageId}')";
        command.Transaction = sqlPersistenceSession.Transaction;
        await command.ExecuteNonQueryAsync(context.CancellationToken);
    }
    #endregion
}