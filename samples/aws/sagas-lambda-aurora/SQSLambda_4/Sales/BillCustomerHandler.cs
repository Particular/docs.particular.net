using NServiceBus.Logging;

public class BillCustomerHandler() : IHandleMessages<OrderReceived>
{
    static ILog logger = LogManager.GetLogger<BillCustomerHandler>();

    public async Task Handle(OrderReceived message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Billing customer for order {0}.", message.OrderId);

        await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 5)), context.CancellationToken);

        await context.Publish(new CustomerBilled { OrderId = message.OrderId });
    }
}