using Shared;

namespace Sales;

public class AddItemHandler(ILogger<AddItemHandler> logger) : IHandleMessages<AddItem>
{
    public Task Handle(AddItem message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing AddItem, Added {item} to {order}", message.Name, message.OrderId);

        var sessionState = context.GetSessionState();

        return Task.CompletedTask;
    }
}
