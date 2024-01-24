namespace V1.Subscriber;

using Contracts;
using NServiceBus.Logging;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    static readonly ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        log.Info($"Something happened with some data '{message.SomeData}'");
        return Task.CompletedTask;
    }
}