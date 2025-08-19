using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NServiceBus;
namespace Raw
{

    public class OrderSubmittedHandler(ILogger<OrderSubmittedHandler> logger) :
        IHandleMessages<OrderSubmitted>
    {
        public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            logger.LogInformation("Order {OrderId} worth {Value} persisted by raw sql", message.OrderId, message.Value);

            #region StoreDataRaw

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

                parameters.AddWithValue("Id", $"Raw-{message.OrderId}");
                parameters.AddWithValue("Value", message.Value);

                await command.ExecuteNonQueryAsync(context.CancellationToken);
            }

            #endregion
        }
    }

}