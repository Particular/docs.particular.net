using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Subscriber1.Contracts;

class Consumer1EventHandler :
    IHandleMessages<Consumer1Contract>
{
    static ILog log = LogManager.GetLogger<Consumer1EventHandler>();

    public Task Handle(Consumer1Contract message, IMessageHandlerContext context)
    {
        log.Info(message.Consumer1Property);
        return Task.CompletedTask;
    }
}