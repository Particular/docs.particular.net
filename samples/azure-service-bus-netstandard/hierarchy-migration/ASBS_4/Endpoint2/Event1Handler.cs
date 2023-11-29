using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Event1Handler :
    IHandleMessages<Event1>
{
    static ILog log = LogManager.GetLogger<Event1Handler>();

    public async Task Handle(Event1 message, IMessageHandlerContext context)
    {
        log.Info($"Received Event1: {message.Property}");

        var event2 = new Event2
        {
            Property = $"({message.EventNumber}) Something happened in Endpoint2",
            EventNumber = message.EventNumber
        };
        await context.Publish(event2);
    }
}