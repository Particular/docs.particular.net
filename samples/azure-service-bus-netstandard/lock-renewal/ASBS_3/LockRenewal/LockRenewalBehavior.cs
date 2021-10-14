using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
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

        var message = context.Extensions.Get<ServiceBusReceivedMessage>();

        #endregion

        #region get-connection-and-path

        var transportTransaction = context.Extensions.Get<TransportTransaction>();
        var serviceBusClient = transportTransaction.Get<ServiceBusClient>();

        #endregion

        var messageReceiver = serviceBusClient.CreateReceiver("Samples.ASB.SendReply.LockRenewal");

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

                    await messageReceiver.RenewMessageLockAsync(message, cancellationToken).ConfigureAwait(false);

                    Log.Info($"Lock renewed till {message.LockedUntil} UTC / {message.LockedUntil.ToLocalTime()} local");
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