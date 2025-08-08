using Microsoft.Extensions.Logging;

public class SecondEventHandler(ILogger<SecondEventHandler> logger) : IHandleMessages<SecondEvent>
{
    public Task Handle(SecondEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received second event #{SomeValue}", message.SomeValue);
        return Task.CompletedTask;
    }
}