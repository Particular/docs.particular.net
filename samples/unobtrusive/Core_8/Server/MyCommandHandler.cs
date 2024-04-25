using Commands;
using NServiceBus.Logging;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    static readonly ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        log.Info($"Command received, id:{message.CommandId}");
        return Task.CompletedTask;
    }
}