using Messages;
using Microsoft.Extensions.Logging;

public class ResponseHandler(ILogger<ResponseHandler> logger) : IHandleMessages<Response>
{
    public Task Handle(Response message, IMessageHandlerContext context)
    {
        logger.LogInformation("Response received from server for request with id:{ResponseId}", message.ResponseId);
        return Task.CompletedTask;
    }
}