namespace Receiver;

using Microsoft.Extensions.Logging;
using Shared;

public class RequestMessageHandler(ILogger<RequestMessageHandler> logger) : IHandleMessages<RequestMessage>
{
    public Task Handle(RequestMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Request received with description: {Data}", message.Data);

        var response = new ResponseMessage
        {
            Id = message.Id,
            Data = message.Data
        };
        return context.Reply(response);
    }
}