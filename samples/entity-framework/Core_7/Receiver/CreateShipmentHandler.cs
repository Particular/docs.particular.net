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

        var shipment = new Shipment
        {
            OrderId = message.OrderId,
            Location = message.ShipTo
        };
        context.DataContext().Shipments.Add(shipment);

        #endregion

        return Task.CompletedTask;
    }
}