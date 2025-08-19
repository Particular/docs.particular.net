using Microsoft.Extensions.Logging;

public class StageInventoryHandler(ILogger<StageInventoryHandler> logger) : IHandleMessages<OrderReceived>
{
    public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        logger.LogInformation("Staging inventory for order {OrderId}.", message.OrderId);

        await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 5)), context.CancellationToken);

        await context.Publish(new InventoryStaged { OrderId = message.OrderId });
    }
}