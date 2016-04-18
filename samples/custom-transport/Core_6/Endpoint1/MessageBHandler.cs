using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region MessageBHandler
public class MessageBHandler : IHandleMessages<MessageB>
{
    static ILog logger = LogManager.GetLogger<MessageBHandler>();

    public Task Handle(MessageB message, IMessageHandlerContext context)
    {
        logger.Info("MessageB Handled");
        return TaskEx.CompletedTask;
    }

}
#endregion