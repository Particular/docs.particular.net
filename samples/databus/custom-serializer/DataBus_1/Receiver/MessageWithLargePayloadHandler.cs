using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class MessageWithLargePayloadHandler(ILogger<MessageWithLargePayloadHandler> logger) :
    IHandleMessages<MessageWithLargePayload>
{

    public Task Handle(MessageWithLargePayload message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received containing {MeasurementCount} measurements", message.LargeData.Value.Length);
        return Task.CompletedTask;
    }
}