using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    static readonly ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        log.Info($"Received {nameof(MyCommand)} with a payload of {commandMessage.Data?.Length ?? 0} bytes.");
        return Task.CompletedTask;
    }
}