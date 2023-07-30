using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region FollowupMessageHandler

public class FollowupMessageHandler : IHandleMessages<FollowupMessage>
{
    static readonly ILog Log = LogManager.GetLogger<FollowupMessageHandler>();

    public async Task Handle(FollowupMessage message, IMessageHandlerContext context)
    {
        Log.Info($"Handling {nameof(FollowupMessage)} in {nameof(FollowupMessageHandler)}");        
        await context.Send(new BackToSenderMessage());
        Log.Info($"Sent {nameof(BackToSenderMessage)}");
    }
}

#endregion