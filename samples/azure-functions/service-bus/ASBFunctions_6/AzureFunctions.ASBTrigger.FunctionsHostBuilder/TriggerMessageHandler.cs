using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region TriggerMessageHandler

public class TriggerMessageHandler(CustomComponent customComponent, ILogger<TriggerMessageHandler> logger) : IHandleMessages<TriggerMessage>
{
  
    public Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        logger.LogWarning($"Handling {nameof(TriggerMessage)} in {nameof(TriggerMessageHandler)}");
        logger.LogWarning($"Custom component returned: {customComponent.GetValue()}");

        return context.SendLocal(new FollowupMessage());
    }
}

#endregion