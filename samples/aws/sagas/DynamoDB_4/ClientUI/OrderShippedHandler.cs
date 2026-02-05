using NServiceBus.Logging;

public class OrderShippedHandler : IHandleMessages<OrderShipped>
{
    static readonly ILog Log = LogManager.GetLogger<OrderShippedHandler>();

    public Task Handle(OrderShipped message, IMessageHandlerContext context)
    {
        Log.Info($"Order {message.OrderId} has shipped.");

        return Task.CompletedTask;
    }
}