namespace Receiver;

using NServiceBus.Logging;
using Shared;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    static readonly ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        log.Info($"MyCommand received from server with data: {message.Data}");
        return Task.CompletedTask;
    }
}
