using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class OrderReceivedHandler :
    IHandleMessages<OrderReceived>
{
    private readonly ILogger<OrderReceivedHandler> logger;

    public OrderReceivedHandler(ILogger<OrderReceivedHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Subscriber has received OrderReceived event with OrderId {message.OrderId}.");
        return Task.CompletedTask;
    }
}