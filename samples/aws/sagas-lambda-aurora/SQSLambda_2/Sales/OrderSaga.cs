
#region OrderSaga

using Microsoft.Extensions.Logging;

public class OrderSaga(ILogger<OrderSaga> logger) : Saga<OrderSagaData>,
  IAmStartedByMessages<PlaceOrder>,
  IHandleMessages<CustomerBilled>,
  IHandleMessages<InventoryStaged>,
  IHandleTimeouts<OrderDelayed>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
         .ToMessage<PlaceOrder>(s => s.OrderId)
         .ToMessage<CustomerBilled>(s => s.OrderId)
         .ToMessage<InventoryStaged>(s => s.OrderId);
    }

    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Placing order: {OrderId}", Data.OrderId);

        await RequestTimeout(context, TimeSpan.FromSeconds(8), new OrderDelayed { OrderId = message.OrderId });

        await context.Publish(new OrderReceived
        {
            OrderId = message.OrderId
        });
    }

    public async Task Handle(CustomerBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("The customer for order {OrderId} has been billed.", Data.OrderId);
        Data.CustomerBilled = true;

        await ShipItIfPossible(context);
    }

    public async Task Handle(InventoryStaged message, IMessageHandlerContext context)
    {
        logger.LogInformation("The inventory for order {OrderId} has been staged.", Data.OrderId);
        Data.InventoryStaged = true;

        await ShipItIfPossible(context);
    }

    public async Task Timeout(OrderDelayed state, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} is slightly delayed.", Data.OrderId);

        await context.Publish(state);
    }

    async Task ShipItIfPossible(IMessageHandlerContext context)
    {
        if (Data is { CustomerBilled: true, InventoryStaged: true })
        {
            logger.LogInformation("Order {OrderId} has been shipped.", Data.OrderId);

            // Send duplicate message for outbox test.
            await context.Publish(new OrderShipped { OrderId = Data.OrderId });
            MarkAsComplete();
        }
    }

}
#endregion