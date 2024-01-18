using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        var sql = @"insert into receiver.SubmittedOrder
                                    (Id, Value)
                        values      (@Id, @Value)";
        using (var command = new SqlCommand(
            cmdText: sql,
            connection: (SqlConnection)session.Connection,
            transaction: (SqlTransaction)session.Transaction))
        {
            var parameters = command.Parameters;
            parameters.AddWithValue("Id", message.OrderId);
            parameters.AddWithValue("Value", message.Value);
            await command.ExecuteNonQueryAsync()
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
