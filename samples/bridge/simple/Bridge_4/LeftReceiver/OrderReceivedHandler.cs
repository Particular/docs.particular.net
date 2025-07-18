using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class OrderReceivedHandler(ILogger<OrderReceivedHandler> logger) :
    IHandleMessages<OrderReceived>
{
    public Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        logger.LogInformation("Subscriber has received OrderReceived event with OrderId {OrderId}.", message.OrderId);
        return Task.CompletedTask;
    }
}