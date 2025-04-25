using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region TriggerMessageHandler
public class TriggerMessageHandler(ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{
    public async Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Handling {nameof(TriggerMessage)} in ServerlessEndpoint.");
        await context.Send(new ResponseMessage());
    }
}
#endregion