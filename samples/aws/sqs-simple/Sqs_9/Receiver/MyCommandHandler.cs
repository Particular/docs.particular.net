using Microsoft.Extensions.Logging;

public class MyCommandHandler(ILogger<MyCommandHandler> logger) : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Received {Command} with a payload of {Length} bytes.", nameof(MyCommand), commandMessage.Data?.Length ?? 0);
        return Task.CompletedTask;
    }
}