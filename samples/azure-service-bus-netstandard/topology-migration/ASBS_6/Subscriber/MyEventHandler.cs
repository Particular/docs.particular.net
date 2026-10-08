using Shared;

public class MyEventHandler(ILogger<MyEventHandler> logger) : IHandleMessages<MyEvent>
{
    public Task Handle(MyEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("{PublishedOnUtc} Received MyEvent with content: {Content}", message.PublishedOnUtc, message.Content);
        return Task.CompletedTask;
    }
}