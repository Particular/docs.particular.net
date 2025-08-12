using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

#region filter-incoming-messages
public class FilterIncomingMessages: Behavior<ITransportReceiveContext>
{
    readonly ISessionKeyProvider sessionKeyProvider;
    private readonly ILogger<FilterIncomingMessages> logger;

    public FilterIncomingMessages(ISessionKeyProvider sessionKeyProvider, ILogger<FilterIncomingMessages> logger)
    {
        this.sessionKeyProvider = sessionKeyProvider;
        this.logger = logger;
    }

    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        if (IsFromCurrentSession(context.Message))
        {
            await next();
        }
        else
        {
            logger.LogInformation("Dropping message {MessageId} as it does not match the current session", context.Message.MessageId);
        }
    }

    bool IsFromCurrentSession(IncomingMessage message)
        => message.Headers.TryGetValue("NServiceBus.SessionKey", out string sessionKey)
           && sessionKey == sessionKeyProvider.SessionKey;

}
#endregion