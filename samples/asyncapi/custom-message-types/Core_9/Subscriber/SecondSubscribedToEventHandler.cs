using Microsoft.Extensions.Logging;

public class SecondSubscribedToEventHandler(ILogger<SecondSubscribedToEvent> logger) : IHandleMessages<SecondSubscribedToEvent>
{
    public Task Handle(SecondSubscribedToEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received second event #{SomeValue}", message.SomeValue);
        return Task.CompletedTask;
    }
}