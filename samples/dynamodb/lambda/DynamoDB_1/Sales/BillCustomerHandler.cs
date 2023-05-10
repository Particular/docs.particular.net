using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Messages;

namespace Sales
{
  public class BillCustomerHandler : IHandleMessages<OrderReceived>
  {
    ILog log = LogManager.GetLogger<StageInventoryHandler>();

    public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
      log.Info("Billing customer.");

      await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(1, 15)), CancellationToken.None);

      await context.Publish(new CustomerBilled() { OrderId = message.OrderId });
    }
  }
}