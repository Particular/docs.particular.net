using NServiceBus.Logging;

public class StageInventoryHandler() : IHandleMessages<OrderReceived>
{
    static ILog logger = LogManager.GetLogger<StageInventoryHandler>();

    public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Staging inventory for order {0}.", message.OrderId);

        await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 5)), context.CancellationToken);

        await context.Publish(new InventoryStaged { OrderId = message.OrderId });
    }
}