using System;
using System.Linq;
using NServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class CreateShipmentHandler(ReceiverDataContext dataContext, ILogger<CreateShipmentHandler> logger) :
    IHandleMessages<OrderSubmitted>
{
    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        #region StoreShipment

        var order = dataContext.Orders.Local.Single(x => x.OrderId == message.OrderId);

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Order = order,
            Location = message.ShipTo
        };
        dataContext.Shipments.Add(shipment);

        #endregion

        logger.LogInformation($"Shipment for {message.OrderId} created.");

        return Task.CompletedTask;
    }
}