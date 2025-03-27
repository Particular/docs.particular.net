using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region NativeMessageHandler

public class NativeMessageHandler(ILogger<NativeMessageHandler> logger) :
    IHandleMessages<NativeMessage>
{
   
    public Task Handle(NativeMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Message content: {message.Content}");
        logger.LogInformation($"Received native message sent on {message.SentOnUtc} UTC");
        return Task.CompletedTask;
    }
}

#endregion