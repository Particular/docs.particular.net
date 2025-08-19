using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class ClientOrderResponseHandler(ILogger<ClientOrderResponseHandler> logger) :
    IHandleMessages<ClientOrderResponse>
{
    public Task Handle(ClientOrderResponse message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received ClientOrderResponse for ID {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}