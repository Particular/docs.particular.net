using System.Data.SqlClient;
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
        var connection = (SqlConnection)session.Connection;
        var transaction = (SqlTransaction)session.Transaction;
        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        var command = "insert into receiver.OrdersDapper (OrderId, Value) values (@OrderId, @Value)";
        return connection.ExecuteAsync(command, order, transaction);

        #endregion
    }
}