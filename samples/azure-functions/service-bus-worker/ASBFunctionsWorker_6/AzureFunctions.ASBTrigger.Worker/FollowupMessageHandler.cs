using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region FollowupMessageHandler

public class FollowupMessageHandler(ILogger<FollowupMessageHandler> logger) : IHandleMessages<FollowupMessage>
{

    public Task Handle(FollowupMessage message, IMessageHandlerContext context)
    {
        logger.LogWarning("Handling {MessageType} in {HandlerType}.", nameof(FollowupMessage), nameof(FollowupMessageHandler));
        return Task.CompletedTask;
    }
}

#endregion