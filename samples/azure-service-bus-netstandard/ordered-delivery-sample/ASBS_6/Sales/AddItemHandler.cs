using Shared;

namespace Sales;

public class AddItemHandler(ILogger<AddItemHandler> logger) : IHandleMessages<AddItem>
{
    public async Task Handle(AddItem message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing AddItem, Added {item} to {order}", message.Name, message.OrderId);

        var sessionState = context.GetSessionState();

        var orderState = await sessionState.Get<OrderState>(context.CancellationToken) ?? new OrderState();

        orderState?.ItemCount += 1;

        await sessionState.Set(orderState, context.CancellationToken);

        logger.LogInformation("Order {OrderId} now has {ItemCount} item(s)", message.OrderId, orderState?.ItemCount);
    }
}
