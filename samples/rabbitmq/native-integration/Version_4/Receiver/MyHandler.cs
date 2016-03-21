using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.InfoFormat("Got `MyMessage` with id: {0}, property value: {1}", context.MessageId, message.SomeProperty);
        return Task.FromResult(0);
    }
}