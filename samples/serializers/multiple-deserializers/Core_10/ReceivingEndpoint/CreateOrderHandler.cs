using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class CreateOrderHandler(ILogger<CreateOrderHandler> logger) :
    IHandleMessages<CreateOrder>
{
    public Task Handle(CreateOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order received. OriginatingEndpoint:" + context.MessageHeaders[Headers.OriginatingEndpoint]);
        return Task.CompletedTask;
    }
}
