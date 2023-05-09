using Messages;

using NServiceBus;
using NServiceBus.Logging;

namespace Sales;

public class StageInventoryHandler : IHandleMessages<OrderReceived>
{
  ILog log = LogManager.GetLogger<StageInventoryHandler>();

  public async Task Handle(OrderReceived message, IMessageHandlerContext context)
  {
    log.Info("Staging inventory.");

    await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(1, 15)), CancellationToken.None);

    await context.Publish(new InventoryStaged() { OrderId = message.OrderId });
  }
}
