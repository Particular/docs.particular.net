using Microsoft.Extensions.Logging;

using Shared;


public class RequestMessageHandler(ILogger<RequestMessageHandler> logger)
        : IHandleMessages<RequestMessage>
{
    readonly ILogger logger = logger;

    public Task Handle(RequestMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Request received with description: {message.Data}");

        var response = new ResponseMessage
        {
            Id = message.Id,
            Data = message.Data
        };
        return context.Reply(response);
    }
}
