using System.Data.SqlClient;
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

        #region StoreUserDataRaw

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();
        var connection = (SqlConnection)session.Connection;
        var transaction = (SqlTransaction)session.Transaction;

        var command = "insert into receiver.OrdersRaw (OrderId, Value) values (@OrderId, @Value)";

        using (var dbCommand = new SqlCommand(command, connection, transaction))
        {
            var parameters = dbCommand.Parameters;
            parameters.AddWithValue("OrderId", message.OrderId);
            parameters.AddWithValue("Value", message.Value);
            await dbCommand.ExecuteNonQueryAsync()
                .ConfigureAwait(false);
        }

        #endregion
    }
}