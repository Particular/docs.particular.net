using Microsoft.Extensions.Logging;

#region NativeMessageHandler

public class NativeMessageHandler(ILogger<NativeMessageHandler> logger) :
    IHandleMessages<NativeMessage>
{

    public Task Handle(NativeMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message content: {Content}", message.Content);
        logger.LogInformation("Received native message sent on {SentOnUtc} UTC", message.SentOnUtc);
        return Task.CompletedTask;
    }
}

#endregion