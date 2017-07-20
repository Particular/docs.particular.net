using System.Diagnostics;
using System.Threading.Tasks;
using HandlerOrdering;
using NServiceBus;
using NServiceBus.Logging;

#region express-order1
public class MessageHandler1 :
    IHandleMessages<MyMessage>,
    IWantToRunAfter<MessageHandler3>
{
    #endregion
    static ILog log = LogManager.GetLogger<MessageHandler1>();
    public static bool ReceivedMessage;

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        ReceivedMessage = true;
        Debug.Assert(MessageHandler3.ReceivedMessage);
        log.Info("MessageHandler1");
        return Task.CompletedTask;
    }
}