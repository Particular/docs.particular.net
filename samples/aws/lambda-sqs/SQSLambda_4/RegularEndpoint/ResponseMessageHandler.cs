using Microsoft.Extensions.Logging;

public class ResponseMessageHandler(ILogger<ResponseMessageHandler> logger) : IHandleMessages<ResponseMessage>
{
    public Task Handle(ResponseMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handling {MessageType} in RegularEndpoint", nameof(ResponseMessage));
        return Task.CompletedTask;
    }
}