namespace V1.Subscriber;

using Contracts;
using Microsoft.Extensions.Logging;
public class SomethingHappenedHandler(ILogger<SomethingHappenedHandler> logger) : IHandleMessages<ISomethingHappened>
{
    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        logger.LogInformation("Something happened with some data '{SomeData}'", message.SomeData);
        return Task.CompletedTask;
    }
}