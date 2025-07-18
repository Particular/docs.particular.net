using Messages;
using Microsoft.Extensions.Logging;
public class RequestMessageHandler(ILogger<RequestMessageHandler> logger) : IHandleMessages<Request>
{
    public Task Handle(Request message, IMessageHandlerContext context)
    {
        logger.LogInformation("Request received with id:{RequestId}", message.RequestId);

        var response = new Response
        {
            ResponseId = message.RequestId
        };
        return context.Reply(response);
    }
}