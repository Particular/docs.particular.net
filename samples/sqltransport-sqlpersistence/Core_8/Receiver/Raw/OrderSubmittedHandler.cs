using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Raw
{

    public class OrderSubmittedHandler :
        IHandleMessages<OrderSubmitted>
    {
        static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

        public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            log.Info($"Order {message.OrderId} worth {message.Value} persisted by raw sql");

            #region StoreDataRaw

            var session = context.SynchronizedStorageSession.SqlPersistenceSession();

            var sql = @"insert into receiver.SubmittedOrder
                                    (Id, Value)
                        values      (@Id, @Value)";
            using (var command = new SqlCommand(
                cmdText: sql,
                connection: (SqlConnection) session.Connection,
                transaction: (SqlTransaction) session.Transaction))
            {
                var parameters = command.Parameters;
                parameters.AddWithValue("Id", $"Raw-{message.OrderId}");
                parameters.AddWithValue("Value", message.Value);
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }

            #endregion
        }
    }

}