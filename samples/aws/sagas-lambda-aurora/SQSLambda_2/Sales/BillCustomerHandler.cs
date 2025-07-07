using Microsoft.Extensions.Logging;

public class BillCustomerHandler(ILogger<BillCustomerHandler> logger) : IHandleMessages<OrderReceived>
{
  public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {

        logger.LogInformation("Billing customer for order {OrderId}.", message.OrderId);

        await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 5)), context.CancellationToken);

        await context.Publish(new CustomerBilled { OrderId = message.OrderId });
    }
}