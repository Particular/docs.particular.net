using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region MessageAHandler
public class MessageAHandler :
    IHandleMessages<MessageA>
{
    static ILog log = LogManager.GetLogger<MessageAHandler>();

    public Task Handle(MessageA message, IMessageHandlerContext context)
    {
        log.Info("MessageA Handled");
        log.Info("Replying with MessageB");
        return context.Reply(new MessageB());
    }
}
#endregion