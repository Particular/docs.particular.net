using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Processing message {message.Id}");
        return FailureSimulator.Invoke();
    }
}