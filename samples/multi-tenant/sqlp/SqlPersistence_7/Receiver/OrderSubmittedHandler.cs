using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        var dataContext = new ReceiverDataContext(session.Connection);
        dataContext.Database.UseTransaction(session.Transaction);

        dataContext.Orders.Add(order);
        await dataContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        await context.Reply(orderAccepted).ConfigureAwait(false);
    }
}