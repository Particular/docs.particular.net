public class MessageBeingSentHandler(ILogger<MessageBeingSentHandler> logger) : IHandleMessages<MessageBeingSent>
{
    public Task Handle(MessageBeingSent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received message #{Number}", message.Number);
        return Task.CompletedTask;
    }
}