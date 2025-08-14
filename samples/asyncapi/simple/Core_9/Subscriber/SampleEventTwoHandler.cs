using Microsoft.Extensions.Logging;
public class SampleEventTwoHandler(ILogger<SampleEventTwoHandler> logger) : IHandleMessages<SampleEventTwo>
{
    public Task Handle(SampleEventTwo message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received sample event two #{SomeValue}", message.SomeValue);
        return Task.CompletedTask;
    }
}