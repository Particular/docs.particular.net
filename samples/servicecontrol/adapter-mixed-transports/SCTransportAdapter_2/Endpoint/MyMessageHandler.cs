using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();
    ChaosGenerator chaos;

    public MyMessageHandler(ChaosGenerator chaos)
    {
        this.chaos = chaos;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Processing message {message.Id}");
        return chaos.Invoke();
    }
}