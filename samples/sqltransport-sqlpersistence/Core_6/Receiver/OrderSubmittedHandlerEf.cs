using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandlerEf :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderLifecycleSaga>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} being persisted by EF");

        #region StoreDataEf

        var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };

        using (var orderContext = new OrderEfContext(session.Connection))
        {
            orderContext.Database.UseTransaction(session.Transaction);
            await orderContext.OrdersEf.AddAsync(order)
                .ConfigureAwait(false);
            await orderContext.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        #endregion
    }

}