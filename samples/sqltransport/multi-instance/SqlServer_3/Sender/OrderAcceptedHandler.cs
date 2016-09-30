using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class OrderAcceptedHandler :
    IHandleMessages<ClientOrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public Task Handle(ClientOrderAccepted message, IMessageHandlerContext context)
    {
        log.Info($"Received ClientOrderAccepted for ID {message.OrderId}");
        return Task.CompletedTask;
    }
}