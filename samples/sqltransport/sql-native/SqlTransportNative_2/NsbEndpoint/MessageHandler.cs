using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler
public class MessageHandler :
    IHandleMessages<SendMessage>
{
    static ILog log = LogManager.GetLogger<SendMessage>();

    public Task Handle(SendMessage message, IMessageHandlerContext context)
    {
        log.Info($"Message received. Property={message.Property}");

        var replyMessage = new ReplyMessage
        {
            Property = "Hello from NsbEndpoint"
        };
        return context.Reply(replyMessage);
    }
}
#endregion