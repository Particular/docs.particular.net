using System;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

#region filter-incoming-messages
public class FilterIncomingMessages : Behavior<ITransportReceiveContext>
{
    readonly ISessionKeyProvider sessionKeyProvider;

    public FilterIncomingMessages(ISessionKeyProvider sessionKeyProvider)
    {
        this.sessionKeyProvider = sessionKeyProvider;
    }

    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        if (IsFromCurrentSession(context.Message))
        {
            await next().ConfigureAwait(false);
        }
        else
        {
            Log.Debug($"Dropping message {context.Message.MessageId} as it does not match the current session");
        }
    }

    bool IsFromCurrentSession(IncomingMessage message)
        => message.Headers.TryGetValue("NServiceBus.SessionKey", out string sessionKey)
           && sessionKey == sessionKeyProvider.SessionKey;

    static ILog Log = LogManager.GetLogger<FilterIncomingMessages>();
}
#endregion