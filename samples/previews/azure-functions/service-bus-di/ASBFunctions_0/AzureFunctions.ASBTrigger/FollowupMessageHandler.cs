﻿using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region FollowupMessageHandler

public class FollowupMessageHandler : IHandleMessages<FollowupMessage>
{
    private static readonly ILog Log = LogManager.GetLogger<FollowupMessageHandler>();

    public Task Handle(FollowupMessage message, IMessageHandlerContext context)
    {
        Log.Warn($"Handling {nameof(FollowupMessage)} in {nameof(FollowupMessageHandler)}.");
        return Task.CompletedTask;
    }
}

#endregion