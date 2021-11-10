using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region TriggerMessageHandler

public class TriggerMessageHandler : IHandleMessages<TriggerMessage>
{
    static readonly ILog Log = LogManager.GetLogger<TriggerMessageHandler>();

    public Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        var rnd = new Random().Next(10);
        if (rnd % 2 != 0)
        {
            throw new Exception("BOOM!");
        }

        Log.Warn($"Handling {nameof(TriggerMessage)} in {nameof(TriggerMessageHandler)}");

        return context.SendLocal(new FollowupMessage());
    }
}

#endregion