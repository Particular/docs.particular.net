using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region MessageBHandler
public class MessageBHandler :
    IHandleMessages<MessageB>
{
    static ILog log = LogManager.GetLogger<MessageBHandler>();

    public Task Handle(MessageB message, IMessageHandlerContext context)
    {
        log.Info("MessageB Handled");
        return Task.FromResult(0);
    }
}
#endregion