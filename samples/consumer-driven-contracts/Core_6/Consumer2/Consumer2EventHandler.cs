using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Subscriber2.Contracts;

class Consumer2EventHandler :
    IHandleMessages<Consumer2Contract>
{
    static ILog log = LogManager.GetLogger<Consumer2EventHandler>();

    public Task Handle(Consumer2Contract message, IMessageHandlerContext context)
    {
        log.Info(message.Consumer2Property);
        return Task.CompletedTask;
    }
}