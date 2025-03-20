namespace V2.Subscriber;

using Contracts;
using Microsoft.Extensions.Logging;

public class SomethingMoreHappenedHandler(ILogger<SomethingMoreHappenedHandler> logger) : IHandleMessages<ISomethingMoreHappened>
{
    public Task Handle(ISomethingMoreHappened message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Something happened with some data '{message.SomeData}' and more information '{message.MoreInfo}'");
        return Task.CompletedTask;
    }
}