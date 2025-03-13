using Commands;
using Microsoft.Extensions.Logging;

public class MyCommandHandler(ILogger<MyCommandHandler> logger) : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Command received, id:{message.CommandId}");
        return Task.CompletedTask;
    }
}