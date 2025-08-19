using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;


public class OrderCompletedHandler(ILogger<OrderCompletedHandler> logger) :
    IHandleMessages<OrderCompleted>
{
   
    public Task Handle(OrderCompleted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received OrderCompleted for OrderId {message.OrderId}");
        return Task.CompletedTask;
    }
}