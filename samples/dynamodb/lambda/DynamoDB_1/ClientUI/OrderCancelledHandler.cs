using System.Threading.Tasks;

using NServiceBus;
using NServiceBus.Logging;

public class OrderCancelledHandler : IHandleMessages<OrderCancelled>
{
  static readonly ILog Log = LogManager.GetLogger<OrderCancelledHandler>();

  public Task Handle(OrderCancelled message, IMessageHandlerContext context)
  {
    Log.Info($"Order {message.OrderId} was cancelled.");

    return Task.CompletedTask;
  }
}

