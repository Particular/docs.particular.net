using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

class LockRenewalBehavior : Behavior<ITransportReceiveContext>
{
    public LockRenewalBehavior(TimeSpan renewLockTokenIn)
    {
        this.renewLockTokenIn = renewLockTokenIn;
    }

    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        #region native-message-access

        var message = context.Extensions.Get<Message>();
        var lockToken = message.SystemProperties.LockToken;

        #endregion

        #region get-connection-and-path

        var transportTransaction = context.Extensions.Get<TransportTransaction>();
        var (serviceBusConnection, path) = transportTransaction.Get<(ServiceBusConnection, string)>();

        #endregion

        var messageReceiver = new MessageReceiver(serviceBusConnection, path);

        var cts = new CancellationTokenSource();
        var token = cts.Token;

        log.Info($"Incoming message ID: {message.MessageId}");

        #region renewal-background-task

        _ = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    log.Info($"Lock will be renewed in {renewLockTokenIn}");

                    await Task.Delay(renewLockTokenIn, token).ConfigureAwait(false);

                    var time = await messageReceiver.RenewLockAsync(lockToken).ConfigureAwait(false);

                    log.Info($"Lock renewed till {time} UTC / {time.ToLocalTime()} local");
                }
            }
            catch (TaskCanceledException)
            {
                log.Info($"Lock renewal task for incoming message ID: {message.MessageId} was cancelled.");
            }
            catch (Exception exception)
            {
                log.Error($"Failed to renew lock for incoming message ID: {message.MessageId}", exception);
            }
        }, token);

        #endregion

        #region processing-and-cancellation

        try
        {
            await next().ConfigureAwait(false);
        }
        finally
        {
            log.Info($"Cancelling renewal task for incoming message ID: {message.MessageId}");
            cts.Cancel();
        }

        #endregion
    }

    readonly TimeSpan renewLockTokenIn;
    static ILog log = LogManager.GetLogger<LockRenewalBehavior>();
}