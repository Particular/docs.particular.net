using Microsoft.Extensions.Logging;

public class FirstEventHandler(ILogger<FirstEventHandler> logger) : IHandleMessages<FirstEvent>
{
    public Task Handle(FirstEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received first event #{SomeOtherValue}", message.SomeOtherValue);
        return Task.CompletedTask;
    }
}