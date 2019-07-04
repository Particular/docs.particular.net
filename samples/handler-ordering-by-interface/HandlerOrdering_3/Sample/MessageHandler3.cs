using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MessageHandler3 :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MessageHandler3>();
    public static bool ReceivedMessage;

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        ReceivedMessage = true;
        log.Info("MessageHandler3");
        return Task.CompletedTask;
    }
}