using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region IntMessageHandler

public class IntMessageHandler :
    IHandleMessages<IntMessage>
{
    static ILog log = LogManager.GetLogger<IntMessage>();

    public Task Handle(IntMessage message, IMessageHandlerContext context)
    {
        log.Info("Message received, Returning");
        return context.Reply(10);
    }
}

#endregion