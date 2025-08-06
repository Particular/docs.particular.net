using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class CompleteOrderHandler(ReceiverDataContext dataContext, ILogger<CompleteOrderHandler> logger)
        : IHandleMessages<CompleteOrder>
{
    
    public async Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        var order = await dataContext.Orders.FindAsync(message.OrderId, context.CancellationToken);
        var shipment = await dataContext.Shipments.FirstAsync(x => x.Order.OrderId == order.OrderId, context.CancellationToken);

        logger.LogInformation($"Completing order {order.OrderId} worth {order.Value} by shipping to {shipment.Location}.");
    }
} 