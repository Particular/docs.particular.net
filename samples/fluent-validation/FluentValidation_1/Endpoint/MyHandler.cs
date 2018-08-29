using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"MyMessage handler invoked. MyMessage.Content is: {message.Content}");
        return Task.CompletedTask;
    }
}