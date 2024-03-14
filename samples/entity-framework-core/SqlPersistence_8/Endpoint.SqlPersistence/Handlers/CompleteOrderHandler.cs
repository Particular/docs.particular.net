using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;

public class CompleteOrderHandler(ReceiverDataContext dataContext)
        : IHandleMessages<CompleteOrder>
{
    static readonly ILog log = LogManager.GetLogger<CompleteOrderHandler>();

    public async Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        var order = await dataContext.Orders.FindAsync(message.OrderId, context.CancellationToken);
        var shipment = await dataContext.Shipments.FirstAsync(x => x.Order.OrderId == order.OrderId, context.CancellationToken);

        log.Info($"Completing order {order.OrderId} worth {order.Value} by shipping to {shipment.Location}.");
    }
}