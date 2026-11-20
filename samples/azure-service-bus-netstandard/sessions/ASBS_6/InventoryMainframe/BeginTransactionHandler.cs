using Shared;

namespace Inventory;

public class BeginTransactionHandler(ILogger<BeginTransactionHandler> logger) : IHandleMessages<BeginTransaction>
{
    public Task Handle(BeginTransaction message, IMessageHandlerContext context)
    {
        logger.LogInformation("Beginning transaction");
        return Task.CompletedTask;
    }
}