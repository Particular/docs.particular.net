using System.Diagnostics;
using System.Threading.Tasks;
using HandlerOrdering;
using NServiceBus;
using NServiceBus.Logging;


#region express-order2
public class MessageHandler2 :
    IHandleMessages<MyMessage>,
    IWantToRunAfter<MessageHandler1>
{
    #endregion
    static ILog log = LogManager.GetLogger<MessageHandler2>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Debug.Assert(MessageHandler1.ReceivedMessage);
        log.Info("MessageHandler2");
        return Task.CompletedTask;
    }
}