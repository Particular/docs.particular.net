﻿using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region TriggerMessageHandler

public class TriggerMessageHandler : IHandleMessages<TriggerMessage>
{
    static readonly ILog Log = LogManager.GetLogger<TriggerMessageHandler>();

    public Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        Log.Info($"Handling {nameof(TriggerMessage)} in {nameof(TriggerMessageHandler)}");
        return context.SendLocal(new FollowupMessage());
    }
}

#endregion