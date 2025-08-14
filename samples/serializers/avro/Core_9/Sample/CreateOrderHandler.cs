using Microsoft.Extensions.Logging;

public class CreateOrderHandler(ILogger<CreateOrderHandler> logger) :
    IHandleMessages<CreateOrder>
{
    public Task Handle(CreateOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order {message.OrderId} received");
        return Task.CompletedTask;
    }
}