using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Got `MyMessage` with id: {context.MessageId}, property value: {message.SomeProperty}");
        return Task.CompletedTask;
    }
}