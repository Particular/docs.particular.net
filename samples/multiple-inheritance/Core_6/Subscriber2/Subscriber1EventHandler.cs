using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Subscriber2.Contracts;

class Subscriber1EventHandler : 
    IHandleMessages<Subscriber2Event>
{
    static ILog log = LogManager.GetLogger<Subscriber1EventHandler>();

    public Task Handle(Subscriber2Event message, IMessageHandlerContext context)
    {
        log.Info(message.Subscriber2Property);
        return Task.CompletedTask;
    }
}