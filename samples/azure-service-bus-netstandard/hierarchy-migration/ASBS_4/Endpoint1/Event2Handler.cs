using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Event2Handler :
    IHandleMessages<Event2>
{
    static ILog log = LogManager.GetLogger<Event2Handler>();

    public Task Handle(Event2 message, IMessageHandlerContext context)
    {
        log.Info($"Received Event2: {message.Property}");
        return Task.CompletedTask;
    }
}