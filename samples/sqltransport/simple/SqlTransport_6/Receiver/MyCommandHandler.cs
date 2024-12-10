using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    static ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        log.Info($"Hello from {nameof(MyCommandHandler)}");
        return Task.CompletedTask;
    }
}