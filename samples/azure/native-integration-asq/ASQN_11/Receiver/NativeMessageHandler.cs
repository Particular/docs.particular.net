using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class NativeMessageHandler : IHandleMessages<NativeMessage>
{
    static ILog log = LogManager.GetLogger<NativeMessageHandler>();

    public Task Handle(NativeMessage message, IMessageHandlerContext context)
    {
        log.Info($"Message content: {message.Content}");

        return Task.CompletedTask;
    }
}