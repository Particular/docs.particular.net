using Messages;
using Microsoft.Extensions.Logging;

public class MessageThatExpiresHandler(ILogger<MessageThatExpiresHandler> logger) : IHandleMessages<MessageThatExpires>
{

    public Task Handle(MessageThatExpires message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Message [{message.GetType()}] received, id: [{message.RequestId}]");
        return Task.CompletedTask;
    }
}