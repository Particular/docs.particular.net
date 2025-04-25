using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region TriggerMessageHandler

public class TriggerMessageHandler(ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{
  
    public Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogWarning($"Handling {nameof(TriggerMessage)} in {nameof(TriggerMessageHandler)}");

        return context.SendLocal(new FollowupMessage());
    }
}

#endregion