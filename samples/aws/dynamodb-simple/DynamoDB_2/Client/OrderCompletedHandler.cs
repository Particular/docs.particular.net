using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class OrderCompletedHandler(ILogger<OrderCompletedHandler> logger) :
    IHandleMessages<OrderCompleted>
{

    public Task Handle(OrderCompleted message, IMessageHandlerContext context)
    {

        logger.LogInformation("Received OrderCompleted for OrderId {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}