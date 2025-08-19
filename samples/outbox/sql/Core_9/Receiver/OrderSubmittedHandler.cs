using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NServiceBus;
using Microsoft.Extensions.Logging;

sealed class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    private readonly ILogger<OrderSubmittedHandler> logger;

    public OrderSubmittedHandler(ILogger<OrderSubmittedHandler> logger)
    {
        this.logger = logger;
    }

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} worth {OrderValue} submitted",
            message.OrderId,
            message.Value
        );

        #region StoreUserData

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        var sql = @"insert into receiver.SubmittedOrder
                                    (Id, Value)
                        values      (@Id, @Value)";

        await using (var command = new SqlCommand(
                         cmdText: sql,
                         connection: (SqlConnection)session.Connection,
                         transaction: (SqlTransaction)session.Transaction))
        {
            var parameters = command.Parameters;
            parameters.AddWithValue("Id", message.OrderId);
            parameters.AddWithValue("Value", message.Value);
            await command.ExecuteNonQueryAsync(context.CancellationToken);
        }

        #endregion

        #region Reply

        var orderAccepted = new OrderAccepted(OrderId: message.OrderId);
        await context.Reply(orderAccepted);

        #endregion
    }
}