using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;

namespace OrmLite
{

    public class OrderSubmittedHandler :
        IHandleMessages<OrderSubmitted>
    {
        static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

        static OrderSubmittedHandler()
        {
            OrmLiteConfig.DialectProvider = new SqlServer2016OrmLiteDialectProvider();
        }

        public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            log.Info($"Order {message.OrderId} worth {message.Value} persisted by OrmLite");

            #region StoreDataOrmLite

            var session = context.SynchronizedStorageSession.SqlPersistenceSession();

            var order = new SubmittedOrder
            {
                Id = $"OrmLite-{message.OrderId}",
                Value = message.Value,
            };
            return OrmLiteWriteApiAsync.UpdateAsync(
                dbConn: session.Connection,
                obj: order,
                commandFilter: command =>
                {
                    command.Transaction = session.Transaction;
                });

            #endregion
        }
    }
}