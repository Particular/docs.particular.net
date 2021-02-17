using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region NativeMessageHandler

public class NativeMessageHandler :
    IHandleMessages<NativeMessage>
{
    static ILog log = LogManager.GetLogger<NativeMessageHandler>();

    public Task Handle(NativeMessage message, IMessageHandlerContext context)
    {
        log.Info($"Message content: {message.Content}");
        log.Info($"Received native message sent on {message.SentOnUtc} UTC");
        return Task.CompletedTask;
    }
}

#endregion