﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;

public class CompleteOrderHandler
    : IHandleMessages<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<CompleteOrderHandler>();

    public async Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        var dataContext = context.DataContext();

        var order = await dataContext.Orders.FindAsync(message.OrderId)
            .ConfigureAwait(false);
        var shipment = await dataContext.Shipments.FirstAsync(x => x.Order.OrderId == order.OrderId)
            .ConfigureAwait(false);

        log.Info($"Completing order {order.OrderId} worth {order.Value} by shipping to {shipment.Location}.");
    }
}