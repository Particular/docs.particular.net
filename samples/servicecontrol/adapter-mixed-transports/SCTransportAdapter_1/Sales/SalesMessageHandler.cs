using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class SalesMessageHandler :
    IHandleMessages<SalesMessage>
{
    static ILog log = LogManager.GetLogger<SalesMessageHandler>();
    ChaosGenerator chaos;

    public SalesMessageHandler(ChaosGenerator chaos)
    {
        this.chaos = chaos;
    }

    public Task Handle(SalesMessage message, IMessageHandlerContext context)
    {
        log.Info($"Processing message {message.Id}");
        return chaos.Invoke();
    }
}