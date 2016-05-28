using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region NativeMessageHandler

public class NativeMessageHandler : IHandleMessages<NativeMessage>
{
    ILog logger = LogManager.GetLogger<NativeMessageHandler>();

    public Task Handle(NativeMessage message, IMessageHandlerContext context)
    {
        logger.Info($"Message content: {message.Content}");
        logger.Info($"Received native message sent on {message.SendOnUtc} UTC");
        return Task.FromResult(0);
    }
}

#endregion