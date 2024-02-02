public class MyMessageHandler(ILogger<MyMessageHandler> logger) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received message #{message.Number}");
        return Task.CompletedTask;
    }
}