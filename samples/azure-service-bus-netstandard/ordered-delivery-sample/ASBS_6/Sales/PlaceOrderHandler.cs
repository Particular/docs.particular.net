using Shared;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing PlaceOrder for {order}", message.OrderId);

        var sessionState = context.GetSessionState();

        var orderState = await sessionState.Get<OrderState>(context.CancellationToken) ?? new OrderState();

        orderState?.Sum += 1;

        await sessionState.Set(orderState, context.CancellationToken);

        var publishOptions = new PublishOptions();
        publishOptions.SetSessionId(message.OrderId.ToString());

        await context.Publish(new OrderPlaced { OrderId = message.OrderId }, publishOptions);
    }

    public class OrderState
    {
        public int Sum { get; set; }
    }
}