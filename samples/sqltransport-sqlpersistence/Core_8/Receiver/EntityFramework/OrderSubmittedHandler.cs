using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;

namespace EntityFramework
{
    public class OrderSubmittedHandler :
        IHandleMessages<OrderSubmitted>
    {
        static readonly ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

        public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            log.Info($"Order {message.OrderId} worth {message.Value} persisted by EF");

            #region StoreDataEf

            var session = context.SynchronizedStorageSession.SqlPersistenceSession();

            var order = new SubmittedOrder
            {
                Id = $"EF-{message.OrderId}",
                Value = message.Value,
            };

            using (var dbContext = new SubmittedOrderDbContext(session.Connection))
            {
                dbContext.Database.UseTransaction(session.Transaction);

                await dbContext.SubmittedOrder.AddAsync(order, context.CancellationToken)
                    .ConfigureAwait(false);

                await dbContext.SaveChangesAsync(context.CancellationToken)
                    .ConfigureAwait(false);
            }

            #endregion
        }
    }
}