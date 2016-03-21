using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received MyMessage. Property1:'{message.Property1}'. Property2:'{message.Property2}'");
        return Task.FromResult(0);
    }
}