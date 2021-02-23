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

        Log.Info($"Incoming message ID: {message.MessageId}");

        _ = RenewLockToken(token);

        #region processing-and-cancellation

        try
        {
            await next().ConfigureAwait(false);
        }
        finally
        {
            Log.Info($"Cancelling renewal task for incoming message ID: {message.MessageId}");
            cts.Cancel();
            cts.Dispose();
        }

        #endregion

        #region renewal-background-task

        async Task RenewLockToken(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Log.Info($"Lock will be renewed in {renewLockTokenIn}");

                    await Task.Delay(renewLockTokenIn, cancellationToken).ConfigureAwait(false);

                    var time = await messageReceiver.RenewLockAsync(lockToken).ConfigureAwait(false);

                    Log.Info($"Lock renewed till {time} UTC / {time.ToLocalTime()} local");
                }
            }
            catch (OperationCanceledException)
            {
                Log.Info($"Lock renewal task for incoming message ID: {message.MessageId} was cancelled.");
            }
            catch (Exception exception)
            {
                Log.Error($"Failed to renew lock for incoming message ID: {message.MessageId}", exception);
            }
        }

        #endregion
    }

    readonly TimeSpan renewLockTokenIn;
    static readonly ILog Log = LogManager.GetLogger<LockRenewalBehavior>();
}