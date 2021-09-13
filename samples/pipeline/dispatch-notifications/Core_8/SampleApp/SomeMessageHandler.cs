using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class SomeMessageHandler :
    IHandleMessages<SomeMessage>
{
    static ILog log = LogManager.GetLogger<SomeMessageHandler>();

    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        log.Info("Got SomeMessage");
        return Task.CompletedTask;
    }
}