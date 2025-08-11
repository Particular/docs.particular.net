using Microsoft.Extensions.Logging;

public class FirstSubscribedToEventHandler(ILogger<FirstSubscribedToEventHandler> logger) : IHandleMessages<FirstSubscribedToEvent>
{
    public Task Handle(FirstSubscribedToEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received first event #{SomeOtherValue}", message.SomeOtherValue);
        return Task.CompletedTask;
    }
}