using NServiceBus.Logging;

public class SimpleMessageHandler :
    IHandleMessages<SimpleMessage>
{
    static ILog log = LogManager.GetLogger<SimpleMessageHandler>();

    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        #region ReceiverHandler

        log.Info($"Received message with Id = {message.Id}.");
        if (message.Id.Any(char.IsLower))
        {
            throw new Exception("Lowercase characters are not allowed in message Id.");
        }
        log.Info($"Successfully processed message with Id = {message.Id}.");
        return Task.CompletedTask;

        #endregion
    }
}