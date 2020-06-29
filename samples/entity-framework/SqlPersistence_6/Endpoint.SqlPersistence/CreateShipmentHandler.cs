using System;
using System.Linq;
using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class CreateShipmentHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<CreateShipmentHandler>();
    ReceiverDataContext dataContext;

    public CreateShipmentHandler(ReceiverDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
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
        await dataContext.SaveChangesAsync().ConfigureAwait(false);

        #endregion

        log.Info($"Shipment for {message.OrderId} created.");
    }
}