namespace Sender;

using Microsoft.Extensions.Logging;
using Shared;

public class ResponseMessageHandler(ILogger<ResponseMessageHandler> logger) : IHandleMessages<ResponseMessage>
{
    public Task Handle(ResponseMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Response received with description: {Data}", message.Data);
        return Task.CompletedTask;
    }
}