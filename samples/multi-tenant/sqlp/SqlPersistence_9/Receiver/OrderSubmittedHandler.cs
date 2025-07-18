using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class OrderSubmittedHandler(ILogger<OrderSubmittedHandler> logger) :
    IHandleMessages<OrderSubmitted>
{
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} worth {Value} submitted", message.OrderId, message.Value);

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        var session = context.SynchronizedStorageSession.SqlPersistenceSession();

        var dataContext = new ReceiverDataContext(session.Connection);
        dataContext.Database.UseTransaction(session.Transaction);

        dataContext.Orders.Add(order);
        await dataContext.SaveChangesAsync(context.CancellationToken);

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        await context.Reply(orderAccepted);
    }
}