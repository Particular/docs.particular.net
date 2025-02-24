using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;


public class OrderCompletedHandler :
    IHandleMessages<OrderCompleted>
{
    private readonly ILogger<OrderCompletedHandler> logger;

    public OrderCompletedHandler(ILogger<OrderCompletedHandler> logger)
    {
        this.logger = logger;
    }
    public Task Handle(OrderCompleted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received OrderCompleted for OrderId {message.OrderId}");
        return Task.CompletedTask;
    }
}