using Shared;

namespace Inventory;

public class ReserveHandler(ILogger<ReserveHandler> logger) : IHandleMessages<Reserve>
{
    public Task Handle(Reserve message, IMessageHandlerContext context)
    {
        logger.LogInformation("Reserving item");
        return Task.CompletedTask;
    }
}