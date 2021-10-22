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
        Log.Warn($"Handling {nameof(TriggerMessage)} in {nameof(TriggerMessageHandler)}");

        var options = new SendOptions();
        options.RouteToThisEndpoint();
        options.DelayDeliveryWith(TimeSpan.FromSeconds(10));

        return context.Send(new FollowupMessage(), options);
    }
}

#endregion