using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Receiver;

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