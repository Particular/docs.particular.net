namespace Receiver;

using NServiceBus;
using NServiceBus.Logging;
using Shared;

public class MyEventHandler : IHandleMessages<MyEvent>
{
    static readonly ILog log = LogManager.GetLogger<MyEventHandler>();

    public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
    {
        log.Info($"Hello from {nameof(MyEventHandler)}");

        return Task.CompletedTask;
    }
}