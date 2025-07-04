using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace EntityFramework
{
    public class OrderSubmittedHandler(ILogger<OrderSubmittedHandler> logger) :
        IHandleMessages<OrderSubmitted>
    {
        public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
        {
            logger.LogInformation("Order {OrderId} worth {Value} persisted by EF", message.OrderId, message.Value);

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

                await dbContext.SubmittedOrder.AddAsync(order, context.CancellationToken);

                await dbContext.SaveChangesAsync(context.CancellationToken);
            }

            #endregion
        }
    }
}