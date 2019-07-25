using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"MyMessage received from server with data: {message.Data}");
        return Task.CompletedTask;
    }
}