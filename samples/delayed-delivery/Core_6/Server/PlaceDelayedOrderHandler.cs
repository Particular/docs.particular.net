using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
#region PlaceDelayedOrderHandler

public class PlaceDelayedOrderHandler : IHandleMessages<PlaceDelayedOrder>
{
    static ILog log = LogManager.GetLogger<PlaceDelayedOrderHandler>();

    public Task Handle(PlaceDelayedOrder message, IMessageHandlerContext context)
    {
        log.Info($"[Defer Message Delivery] Order for Product:{message.Product} placed with id: {message.Id}");
        return Task.FromResult(0);
    }
}

#endregion
