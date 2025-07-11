using Messages;
using Microsoft.Extensions.Logging;

public class LargeMessageHandler(ILogger<LargeMessageHandler> logger) : IHandleMessages<LargeMessage>
{
    public Task Handle(LargeMessage message, IMessageHandlerContext context)
    {
        if (message.LargeDataBus == null)
        {
            logger.LogInformation("Message [{MessageType}] received, id:{RequestId}", message.GetType(), message.RequestId);
        }
        else
        {
            logger.LogInformation("Message [{MessageType}] received, id:{RequestId} and payload {PayloadLength} bytes", message.GetType(), message.RequestId, message.LargeDataBus.Length);
        }
        return Task.CompletedTask;
    }
}
