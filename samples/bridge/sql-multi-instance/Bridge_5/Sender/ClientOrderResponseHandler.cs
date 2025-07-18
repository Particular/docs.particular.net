using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class ClientOrderResponseHandler :
    IHandleMessages<ClientOrderResponse>
{
    static ILog log = LogManager.GetLogger<ClientOrderResponseHandler>();

    public Task Handle(ClientOrderResponse message, IMessageHandlerContext context)
    {
        log.Info($"Received ClientOrderResponse for ID {message.OrderId}");
        return Task.CompletedTask;
    }
}