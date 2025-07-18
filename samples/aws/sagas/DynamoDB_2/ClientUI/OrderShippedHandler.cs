using Microsoft.Extensions.Logging;
using NServiceBus;

public class OrderShippedHandler(ILogger<OrderShippedHandler> logger) : IHandleMessages<OrderShipped>
{
  public Task Handle(OrderShipped message, IMessageHandlerContext context)
  {
    logger.LogInformation("Order {OrderId} has shipped.", message.OrderId);

    return Task.CompletedTask;
  }
}