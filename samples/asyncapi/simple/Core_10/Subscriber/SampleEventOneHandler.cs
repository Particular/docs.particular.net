using Microsoft.Extensions.Logging;
public class SampleEventOnetHandler(ILogger<SampleEventOnetHandler> logger) : IHandleMessages<SampleEventOne>
{
    public Task Handle(SampleEventOne message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received sample event one #{SomeValue}", message.SomeValue);
        return Task.CompletedTask;
    }
}