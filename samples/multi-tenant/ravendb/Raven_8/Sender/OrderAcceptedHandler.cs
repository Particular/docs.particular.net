using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderAcceptedHandler :
    IHandleMessages<OrderAccepted>
{
    static readonly ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} accepted.");
        return Task.CompletedTask;
    }
}