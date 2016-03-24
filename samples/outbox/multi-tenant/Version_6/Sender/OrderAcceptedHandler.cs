using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        log.InfoFormat("Order {0} accepted.", message.OrderId);

        return Task.FromResult(0);
    }
}