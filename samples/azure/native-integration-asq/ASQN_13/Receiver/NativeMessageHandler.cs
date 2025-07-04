using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class NativeMessageHandler(ILogger<NativeMessageHandler> logger) : IHandleMessages<NativeMessage>
{
    public Task Handle(NativeMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message content: {Content}", message.Content);

        return Task.CompletedTask;
    }
}