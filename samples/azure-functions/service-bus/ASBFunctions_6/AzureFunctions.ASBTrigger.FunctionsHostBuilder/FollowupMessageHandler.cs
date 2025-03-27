using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region FollowupMessageHandler

public class FollowupMessageHandler(CustomComponent customComponent, ILogger<FollowupMessageHandler> logger) : IHandleMessages<FollowupMessage>
{
   
    public Task Handle(FollowupMessage message, IMessageHandlerContext context)
    {
        logger.LogWarning($"Handling {nameof(FollowupMessage)} in {nameof(FollowupMessageHandler)}.");
        logger.LogWarning($"Custom component returned: {customComponent.GetValue()}");

        return Task.CompletedTask;
    }
}

#endregion