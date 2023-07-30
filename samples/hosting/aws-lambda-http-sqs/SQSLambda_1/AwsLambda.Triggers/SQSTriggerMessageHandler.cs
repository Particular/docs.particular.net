using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region TriggerMessageHandler

public class SQSTriggerMessageHandler : IHandleMessages<SQSTriggerMessage>
{
    static readonly ILog Log = LogManager.GetLogger<SQSTriggerMessageHandler>();

    public async Task Handle(SQSTriggerMessage message, IMessageHandlerContext context)
    {
        Log.Info($"Handling {nameof(SQSTriggerMessage)} in {nameof(SQSTriggerMessageHandler)}");
        await context.SendLocal(new FollowupMessage());
        Log.Info($"Sent {nameof(FollowupMessage)} to the current serverless endppoint");
    }
}

#endregion