using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class MessageWithLargePayloadHandler(ILogger<MessageWithLargePayloadHandler> logger) :
    IHandleMessages<MessageWithLargePayload>
{
  
    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Message received containing {message.LargeData.Value.Length} measurements");
        return Task.CompletedTask;
    }
}