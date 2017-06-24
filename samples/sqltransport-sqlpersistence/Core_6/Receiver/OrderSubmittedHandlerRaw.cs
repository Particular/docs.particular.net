using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandlerRaw :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} being persisted by raw sql");

        #region StoreDataRaw

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        var sql = @"insert into receiver.OrdersRaw
                                (OrderId, Value)
                    values      (@OrderId, @Value)";
        using (var command = session.Connection.CreateCommand())
        {
            command.CommandText = sql;
            command.Transaction = session.Transaction;
            command.AddParameter("OrderId", message.OrderId);
            command.AddParameter("Value", message.Value);
            await command.ExecuteNonQueryAsync()
                .ConfigureAwait(false);
        }

        #endregion
    }
}