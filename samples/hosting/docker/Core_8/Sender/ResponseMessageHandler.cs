using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Sender;

public class ResponseMessageHandler(ILogger<ResponseMessageHandler> logger) : IHandleMessages<ResponseMessage>
{
    public Task Handle(ResponseMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Response received with description: {Data}", message.Data);
        return Task.CompletedTask;
    }
}