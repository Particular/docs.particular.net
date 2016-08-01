using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        log.Info("Order billed.");
        return Task.FromResult(0);
    }

    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
}