using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
#region PlaceDelayedOrderHandler

public class PlaceDelayedOrderHandler : IHandleMessages<PlaceDelayedOrder>
{
    static ILog log = LogManager.GetLogger<PlaceDelayedOrderHandler>();

    public Task Handle(PlaceDelayedOrder message, IMessageHandlerContext context)
    {
        log.InfoFormat("[Defer Message Delivery] Order for Product:{0} placed with id: {1}", message.Product, message.Id);
        return Task.FromResult(0);
    }
}

#endregion
