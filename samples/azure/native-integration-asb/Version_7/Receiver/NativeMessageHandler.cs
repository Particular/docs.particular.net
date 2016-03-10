using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region NativeMessageHandler

public class NativeMessageHandler : IHandleMessages<NativeMessage>
{
    ILog logger = LogManager.GetLogger<NativeMessageHandler>();

    public Task Handle(NativeMessage message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Message content: {0}", message.Content);
        logger.InfoFormat("Received native message sent on {0} UTC", message.SendOnUtc);
        return Task.FromResult(0);
    }
}

#endregion