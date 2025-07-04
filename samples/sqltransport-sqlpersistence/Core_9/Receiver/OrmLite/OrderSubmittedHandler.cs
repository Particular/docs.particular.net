using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using ServiceStack.OrmLite;

namespace OrmLite
{

    public class OrderSubmittedHandler(ILogger<OrderSubmittedHandler> logger) :
        IHandleMessages<OrderSubmitted>
    {
        static OrderSubmittedHandler()
        {
            OrmLiteConfig.DialectProvider = SqlServer2016Dialect.Provider;
        }

        public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            logger.LogInformation("Order {OrderId} worth {Value} persisted by OrmLite", message.OrderId, message.Value);

            #region StoreDataOrmLite

            var session = context.SynchronizedStorageSession.SqlPersistenceSession();

            var order = new SubmittedOrder
            {
                Id = $"OrmLite-{message.OrderId}",
                Value = message.Value,
            };

            return session.Connection.UpdateAsync(
                obj: order,
                commandFilter: command => command.Transaction = session.Transaction,
                context.CancellationToken);

            #endregion
        }
    }
}