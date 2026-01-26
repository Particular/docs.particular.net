using NServiceBus.Logging;

public class BillCustomerHandler : IHandleMessages<OrderReceived>
{
    ILog log = LogManager.GetLogger<BillCustomerHandler>();

    public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        log.Info($"Billing customer for order {message.OrderId}.");

        await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 5)), context.CancellationToken);

        await context.Publish(new CustomerBilled { OrderId = message.OrderId });
    }
}
