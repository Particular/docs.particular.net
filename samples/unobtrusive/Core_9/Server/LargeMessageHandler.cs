using Messages;
using Microsoft.Extensions.Logging;

public class LargeMessageHandler(ILogger<LargeMessageHandler> logger) : IHandleMessages<LargeMessage>
{
    public Task Handle(LargeMessage message, IMessageHandlerContext context)
    {
        if (message.LargeDataBus == null)
        {
            logger.LogInformation($"Message [{message.GetType()}] received, id:{message.RequestId}");
        }
        else
        {
            logger.LogInformation($"Message [{message.GetType()}] received, id:{message.RequestId} and payload {message.LargeDataBus.Length} bytes");
        }
        return Task.CompletedTask;
    }
}
