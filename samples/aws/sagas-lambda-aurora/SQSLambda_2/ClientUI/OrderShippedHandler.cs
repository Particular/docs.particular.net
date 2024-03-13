using NServiceBus.Logging;

public class OrderShippedHandler : IHandleMessages<OrderShipped>
{
    readonly ILog log = LogManager.GetLogger<OrderShippedHandler>();

    public Task Handle(OrderShipped message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} has shipped.");

        return Task.CompletedTask;
    }
}