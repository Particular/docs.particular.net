using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyMessageHandler : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Logger.Info("Received message");
        return Task.CompletedTask;
    }

    private static ILog Logger = LogManager.GetLogger<MyMessageHandler>();
}