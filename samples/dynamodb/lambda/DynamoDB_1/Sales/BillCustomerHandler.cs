using System;
using System.Threading;
using System.Threading.Tasks;

using NServiceBus;
using NServiceBus.Logging;

public class BillCustomerHandler : IHandleMessages<OrderReceived>
{
  ILog log = LogManager.GetLogger<BillCustomerHandler>();

  public async Task Handle(OrderReceived message, IMessageHandlerContext context)
  {
    log.Info("Billing customer.");

    await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(1, 5)), CancellationToken.None);

    await context.Publish(new CustomerBilled() { OrderId = message.OrderId });
  }
}
