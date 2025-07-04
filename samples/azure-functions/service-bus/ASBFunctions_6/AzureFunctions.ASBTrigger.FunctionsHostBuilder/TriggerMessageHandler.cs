using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region TriggerMessageHandler

public class TriggerMessageHandler(CustomComponent customComponent, ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{

    public Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogWarning("Handling {MessageType} in {HandlerType}", nameof(TriggerMessage), nameof(TriggerMessageHandler));
        logger.LogWarning("Custom component returned: {Value}", customComponent.GetValue());

        return context.SendLocal(new FollowupMessage());
    }
}

#endregion