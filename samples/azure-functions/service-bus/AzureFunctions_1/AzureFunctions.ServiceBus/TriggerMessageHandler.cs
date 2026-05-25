namespace AzureFunctions.ServiceBus;

using Microsoft.Extensions.Logging;
using NServiceBus;

#region service-bus-trigger-handler
public class TriggerMessageHandler(ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{
    public Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogWarning("Handling {MessageType} in {HandlerType}", nameof(TriggerMessage), nameof(TriggerMessageHandler));

        return context.SendLocal(new FollowupMessage());
    }
}
#endregion
