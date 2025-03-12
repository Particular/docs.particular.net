using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) :
    IHandleMessages<OrderAccepted>
{
  
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order {message.OrderId} accepted.");
        return Task.CompletedTask;
    }
}