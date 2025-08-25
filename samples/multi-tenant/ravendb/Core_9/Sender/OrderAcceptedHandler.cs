using Microsoft.Extensions.Logging;

public class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) :
    IHandleMessages<OrderAccepted>
{
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} accepted.", message.OrderId);
        return Task.CompletedTask;
    }
}