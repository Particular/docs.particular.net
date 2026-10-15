using Shared;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing PlaceOrder for {order}", message.OrderId);

        var sessionState = context.GetSessionState();

        var orderState = await sessionState.Get<OrderState>(context.CancellationToken);

        var numberOfItems = orderState?.ItemCount ?? 0;

        var publishOptions = new PublishOptions();
        publishOptions.SetSessionId(message.OrderId.ToString());

        await context.Publish(new OrderPlaced { OrderId = message.OrderId , NumberOfItems = numberOfItems}, publishOptions);
    }
}