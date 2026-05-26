using Microsoft.Extensions.Logging;

namespace LambdaFunctions;

#region TriggerMessageHandler
class TriggerMessageHandler(ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{
    public async Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Handling {nameof(TriggerMessage)} in ServerlessEndpoint.");
        await context.Send(new ResponseMessage());
    }
}
#endregion
