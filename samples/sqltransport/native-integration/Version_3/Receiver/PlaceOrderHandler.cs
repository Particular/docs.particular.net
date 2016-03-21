using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<LegacyOrderDetectedHandler>();

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.InfoFormat("Order {0} placed", message.OrderId);
        return Task.FromResult(0);
    }

}