using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Faults;
using NServiceBus.Logging;

// ReSharper disable UnusedParameter.Local

#region subscriptions

public static class SubscribeToNotifications
{
    static ILog log = LogManager.GetLogger(typeof(SubscribeToNotifications));

    public static void Subscribe(EndpointConfiguration endpointConfiguration)
    {
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(settings => settings.OnMessageBeingRetried(Log));
        recoverability.Delayed(settings => settings.OnMessageBeingRetried(Log));
        recoverability.Failed(settings => settings.OnMessageSentToErrorQueue(Log));
    }

    static string GetMessageString(byte[] body)
    {
        return Encoding.UTF8.GetString(body);
    }

    static Task Log(FailedMessage failed)
    {
        log.Fatal($@"Message sent to error queue.
        Body:
        {GetMessageString(failed.Body)}");
        return Task.CompletedTask;
    }

    static Task Log(DelayedRetryMessage retry)
    {
        log.Fatal($@"Message sent to Delayed Retries.
        RetryAttempt:{retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
        return Task.CompletedTask;
    }

    static Task Log(ImmediateRetryMessage retry)
    {
        log.Fatal($@"Message sent to Immediate Retry.
        RetryAttempt:{retry.RetryAttempt}
        Body:
        {GetMessageString(retry.Body)}");
        return Task.CompletedTask;
    }
}

#endregion
