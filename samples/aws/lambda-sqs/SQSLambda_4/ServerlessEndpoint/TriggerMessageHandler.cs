using Microsoft.Extensions.Logging;

#region TriggerMessageHandler
public class TriggerMessageHandler(ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{

    public async Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogWarning($"Handling {nameof(TriggerMessage)} in ServerlessEndpoint.");
        await context.Send(new ResponseMessage());
    }
}
#endregion