using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Subscriber1.Contracts;

class Subscriber1EventHandler : 
    IHandleMessages<Subscriber1Event>
{
    static ILog log = LogManager.GetLogger<Subscriber1EventHandler>();

    public Task Handle(Subscriber1Event message, IMessageHandlerContext context)
    {
        log.Info(message.Subscriber1Property);
        return Task.CompletedTask;
    }
}