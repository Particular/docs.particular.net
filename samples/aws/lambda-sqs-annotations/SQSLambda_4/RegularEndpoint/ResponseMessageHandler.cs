using Microsoft.Extensions.Logging;

public class ResponseMessageHandler(ILogger<ResponseMessageHandler> logger) : IHandleMessages<ResponseMessage>
{
    public Task Handle(ResponseMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Handling {nameof(ResponseMessage)} in RegularEndpoint");
        return Task.CompletedTask;
    }
}