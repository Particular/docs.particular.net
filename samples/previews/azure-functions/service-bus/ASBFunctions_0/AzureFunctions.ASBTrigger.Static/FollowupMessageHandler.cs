using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region FollowupMessageHandler

public class FollowupMessageHandler : IHandleMessages<FollowupMessage>
{
    static readonly ILog Log = LogManager.GetLogger<FollowupMessageHandler>();
    readonly CustomComponent customComponent;

    public FollowupMessageHandler(CustomComponent customComponent)
    {
        this.customComponent = customComponent;
    }

    public Task Handle(FollowupMessage message, IMessageHandlerContext context)
    {
        Log.Warn($"Handling {nameof(FollowupMessage)} in {nameof(FollowupMessageHandler)}.");
        Log.Warn($"Custom component returned: {customComponent.GetValue()}");

        return Task.CompletedTask;
    }
}

#endregion