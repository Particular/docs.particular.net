using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region NativeMessageHandler

public class NativeMessageHandler :
    IHandleMessages<NativeMessage>
{
    static readonly ILog Log = LogManager.GetLogger<NativeMessageHandler>();

    public Task Handle(NativeMessage message, IMessageHandlerContext context)
    {
        Log.Info($"Message content: {message.Content}");
        Log.Info($"Received native message sent on {message.SentOnUtc} UTC");
        return Task.CompletedTask;
    }
}

#endregion