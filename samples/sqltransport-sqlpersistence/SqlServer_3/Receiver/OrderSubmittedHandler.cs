using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();
        var connection = session.Connection as SqlConnection;

        const string sqlCommand = "INSERT INTO [receiver].[Orders] (OrderId, Value) VALUES (@OrderId, @Value)";

        using (var dbCommand = new SqlCommand(sqlCommand, connection))
        {
            dbCommand.Parameters.AddWithValue("OrderId", message.OrderId);
            dbCommand.Parameters.AddWithValue("Value", message.Value);
            dbCommand.ExecuteNonQuery();
        }

        #endregion

        #region Reply

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        return context.Reply(orderAccepted);

        #endregion
    }
}