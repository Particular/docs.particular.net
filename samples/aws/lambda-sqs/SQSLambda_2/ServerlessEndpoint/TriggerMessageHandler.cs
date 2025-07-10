using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region TriggerMessageHandler
public class TriggerMessageHandler(ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{
    public async Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handling {MessageType} in ServerlessEndpoint.", nameof(TriggerMessage));
        await context.Send(new ResponseMessage());
    }
}
#endregion