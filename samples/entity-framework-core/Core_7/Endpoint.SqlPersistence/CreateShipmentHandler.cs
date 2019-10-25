using System;
using System.Linq;
using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class CreateShipmentHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<CreateShipmentHandler>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        #region StoreShipment

        var order = context.DataContext().Orders.Local.Single(x => x.OrderId == message.OrderId);

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Order = order,
            Location = message.ShipTo
        };
        context.DataContext().Shipments.Add(shipment);

        #endregion

        log.Info($"Shipment for {message.OrderId} created.");

        return Task.CompletedTask;
    }
}