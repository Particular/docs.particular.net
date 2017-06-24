using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderAcceptedHandler :
    IHandleMessages<OrderReceived>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} accepted.");
        return Task.CompletedTask;
    }
}