using Microsoft.Extensions.Logging;
public class OrderShippedHandler(ILogger<OrderShippedHandler> logger) : IHandleMessages<OrderShipped>
{
    public Task Handle(OrderShipped message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} has shipped.", message.OrderId);

        return Task.CompletedTask;
    }
}