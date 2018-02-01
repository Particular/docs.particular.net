using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class ShippingMessageHandler :
    IHandleMessages<ShippingMessage>
{
    static ILog log = LogManager.GetLogger<ShippingMessageHandler>();
    ChaosGenerator chaos;

    public ShippingMessageHandler(ChaosGenerator chaos)
    {
        this.chaos = chaos;
    }

    public Task Handle(ShippingMessage message, IMessageHandlerContext context)
    {
        log.Info($"Processing message{message.Id}");
        return chaos.Invoke();
    }
}