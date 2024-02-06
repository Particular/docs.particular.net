using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyEventHandler : IHandleMessages<MyEvent>
{
    static readonly ILog log = LogManager.GetLogger<MyEventHandler>();

    public Task Handle(MyEvent eventMessage, IMessageHandlerContext context)
    {
        log.Info($"Received {nameof(MyEvent)} with a payload of {eventMessage.Data?.Length ?? 0} bytes.");
        return Task.CompletedTask;
    }
}