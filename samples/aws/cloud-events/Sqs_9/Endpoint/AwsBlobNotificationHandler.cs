using Microsoft.Extensions.Logging;

public class AwsBlobNotificationHandler(ILogger<AwsBlobNotification> logger) :
    IHandleMessages<AwsBlobNotification>
{
    public Task Handle(AwsBlobNotification message, IMessageHandlerContext context)
    {
        logger.LogInformation("Blob {Key} created!", message.Key);
        return Task.CompletedTask;
    }
}
