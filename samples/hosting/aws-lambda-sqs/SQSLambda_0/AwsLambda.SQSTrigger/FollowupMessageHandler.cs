using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region FollowupMessageHandler

public class FollowupMessageHandler : IHandleMessages<FollowupMessage>
{
    static readonly ILog Log = LogManager.GetLogger<FollowupMessageHandler>();

    public Task Handle(FollowupMessage message, IMessageHandlerContext context)
    {
        Log.Info($"Handling {nameof(FollowupMessage)}.");
        return context.Send(new BackToSenderMessage());
    }
}

#endregion