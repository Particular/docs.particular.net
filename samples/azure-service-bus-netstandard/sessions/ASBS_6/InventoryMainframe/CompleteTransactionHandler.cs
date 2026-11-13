using Shared;

namespace Inventory;

public class CompleteTransactionHandler(ILogger<CompleteTransactionHandler> logger) : IHandleMessages<CompleteTransaction>
{
    public Task Handle(CompleteTransaction message, IMessageHandlerContext context)
    {
        logger.LogInformation("Completing transaction");
        return Task.CompletedTask;
    }
}