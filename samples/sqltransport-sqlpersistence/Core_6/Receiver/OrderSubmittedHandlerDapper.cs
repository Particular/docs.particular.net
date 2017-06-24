using System.Threading.Tasks;
using Dapper;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandlerDapper :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} being persisted by dapper");

        #region StoreDataDapper

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };

        var sql = @"insert into receiver.OrdersDapper
                                (OrderId, Value)
                    values      (@OrderId, @Value)";
        return session.Connection.ExecuteAsync(
            sql: sql,
            param: order,
            transaction: session.Transaction);

        #endregion
    }
}