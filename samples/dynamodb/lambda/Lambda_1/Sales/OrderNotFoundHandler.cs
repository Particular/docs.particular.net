using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Sagas;

using Messages;

namespace Sales;

internal class OrderNotFoundHandler : IHandleSagaNotFound
{
  ILog log = LogManager.GetLogger(typeof(OrderNotFoundHandler));

  public async Task Handle(object message, IMessageProcessingContext context)
  {
    var cancelOrder = message as CancelOrder;

    if (cancelOrder != null)
    {
      log.Warn($"Order {cancelOrder.OrderId} was not found and could not be cancelled.");

      await context.Reply(new OrderCancelled()
      {
        OrderId = cancelOrder.OrderId,
        IsCancelled = false
      });
    }

    var orderShipped = message as OrderShipped;

    if (orderShipped != null)
    {
      log.Warn($"Shipped order {orderShipped.OrderId} was not found.");
    }

    log.Error($"No saga found for type {message.GetType().FullName}");
  }
}
