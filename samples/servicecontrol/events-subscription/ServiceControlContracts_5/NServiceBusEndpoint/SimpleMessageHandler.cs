using Microsoft.Extensions.Logging;

public class SimpleMessageHandler(ILogger<SimpleMessageHandler> logger) :
    IHandleMessages<SimpleMessage>
{
    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received message with Id = {Id}.", message.Id);
        throw new Exception("BOOM!");
    }
}