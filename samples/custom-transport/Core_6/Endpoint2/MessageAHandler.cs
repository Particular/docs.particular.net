using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region MessageAHandler
public class MessageAHandler : IHandleMessages<MessageA>
{
    static ILog logger = LogManager.GetLogger<MessageAHandler>();

    public Task Handle(MessageA message, IMessageHandlerContext context)
    {
        logger.Info("MessageA Handled");
        logger.Info("Replying with MessageB");
        return context.Reply(new MessageB());
    }

}
#endregion