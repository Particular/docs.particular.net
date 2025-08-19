namespace Receiver;

using NServiceBus;
using NServiceBus.Logging;
using Shared;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    static readonly ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        log.Info($"Hello from {nameof(MyCommandHandler)}");

        return Task.CompletedTask;
    }
}