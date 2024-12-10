using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Dapper
{
    public class OrderSubmittedHandler :
        IHandleMessages<OrderSubmitted>
    {
        static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

        public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            log.Info($"Order {message.OrderId} worth {message.Value} persisted by Dapper");

            #region StoreDataDapper

            var session = context.SynchronizedStorageSession.SqlPersistenceSession();

            var order = new SubmittedOrder
            {
                Id = $"Dapper-{message.OrderId}",
                Value = message.Value,
            };

            var sql = @"insert into receiver.SubmittedOrder
                                    (Id, Value)
                        values      (@Id, @Value)";
            return session.Connection.ExecuteAsync(sql: sql,
                param: order,
                transaction: session.Transaction);

            #endregion
        }
    }
}