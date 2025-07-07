using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region TriggerMessageHandler

public class TriggerMessageHandler(ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{

    public Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogWarning("Handling {MessageType} in {HandlerType}", nameof(TriggerMessage), nameof(TriggerMessageHandler));

        return context.SendLocal(new FollowupMessage());
    }
}

#endregion