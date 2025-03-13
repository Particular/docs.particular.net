using System.Threading.Tasks;
using Common.Logging;
using NServiceBus;

public class MyHandler:
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info("Hello from MyHandler");
        return Task.CompletedTask;
    }
}