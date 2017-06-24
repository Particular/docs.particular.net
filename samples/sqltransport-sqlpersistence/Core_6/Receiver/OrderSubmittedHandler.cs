using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();
        var connection = session.Connection as SqlConnection;

        var sqlCommand = "insert into [receiver].[Orders] (OrderId, Value) values (@OrderId, @Value)";

        using (var dbCommand = new SqlCommand(sqlCommand, connection))
        {
            var parameters = dbCommand.Parameters;
            parameters.AddWithValue("OrderId", message.OrderId);
            parameters.AddWithValue("Value", message.Value);
            await dbCommand.ExecuteNonQueryAsync()
                .ConfigureAwait(false);
        }

        #endregion

        #region Reply

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        await context.Reply(orderAccepted)
            .ConfigureAwait(false);

        #endregion
    }
}