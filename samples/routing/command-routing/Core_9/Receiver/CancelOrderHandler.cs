using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

class CancelOrderHandler(ILogger<CancelOrderHandler> logger) :
    IHandleMessages<CancelOrder>
{
    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("CancelOrder command received: {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }

}