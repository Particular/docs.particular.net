using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MyMessageHandler :
    IHandleMessages<IMyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();

    public Task Handle(IMyMessage message, IMessageHandlerContext context)
    {
        log.Info($"IMyMessage received from server with data: {message.Data}");
        return Task.CompletedTask;
    }
}