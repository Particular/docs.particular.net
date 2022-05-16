using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

class LockRenewalBehavior : Behavior<ITransportReceiveContext>
{
    public LockRenewalBehavior(TimeSpan lockDuration, TimeSpan renewLockTokenIn, string queueName)
    {
        this.lockDuration = lockDuration;
        this.renewLockTokenIn = renewLockTokenIn;
        this.queueName = queueName;
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

        var messageReceiver = serviceBusClient.CreateReceiver(queueName);

        try
        {
            var now = DateTimeOffset.UtcNow;
            var remaining = message.LockedUntil - now;

            if (remaining < renewLockTokenIn)
            {
                throw new Exception($"Not processing message because remaining lock duration is below configured renewal interval.")
                {
                    Data = { { "ServiceBusReceivedMessage.MessageId", message.MessageId } }
                };
            }

            var elapsed = lockDuration - remaining;

            if (elapsed > renewLockTokenIn)
            {
                Log.Warn($"{message.MessageId}: Incoming message is locked untill {message.LockedUntil:s}Z but already passed configured renewal interval, renewing lock first. Consider lowering the prefetch count.");
                await messageReceiver.RenewMessageLockAsync(message, context.CancellationToken).ConfigureAwait(false);
            }

            ;
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken))
            {
                var token = cts.Token;

                _ = RenewLockToken(token);

                #region processing-and-cancellation

                try
                {
                    await next().ConfigureAwait(false);
                }
                finally
                {
                    remaining = message.LockedUntil - DateTimeOffset.UtcNow;

                    if (remaining < renewLockTokenIn)
                    {
                        Log.Warn($"{message.MessageId}: Processing completed but LockedUntil {message.LockedUntil:s}Z less than {renewLockTokenIn}. This could indicate issues during lock renewal.");
                    }

                    cts.Cancel();
                }

                #endregion
            }
        }
        finally
        {
            await messageReceiver.DisposeAsync(); // Cannot use "await using" because of lang version 7.3
        }


        #region renewal-background-task

        async Task RenewLockToken(CancellationToken cancellationToken)
        {
            try
            {
                int attempts = 0;

                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(renewLockTokenIn, cancellationToken).ConfigureAwait(false);

                    try
                    {
                        await messageReceiver.RenewMessageLockAsync(message, cancellationToken).ConfigureAwait(false);
                        attempts = 0;
                        Log.Info($"{message.MessageId}: Lock renewed untill {message.LockedUntil:s}Z.");
                    }
                    catch (ServiceBusException e) when (e.Reason == ServiceBusFailureReason.MessageLockLost)
                    {
                        Log.Error($"{message.MessageId}: Lock lost.", e);
                        return;
                    }
                    catch (Exception e) when (!(e is OperationCanceledException))
                    {
                        ++attempts;
                        Log.Warn($"{message.MessageId}: Failed to renew lock (#{attempts:N0}), if lock cannot be renewed within {message.LockedUntil:s}Z message will reappear.", e);
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // Expected, no need to process
            }
            catch (Exception e)
            {
                Log.Fatal($"{message.MessageId}: RenewLockToken: " + e.Message, e);
            }
        }

        #endregion
    }

    readonly TimeSpan lockDuration;
    readonly TimeSpan renewLockTokenIn;
    readonly string queueName;
    static readonly ILog Log = LogManager.GetLogger<LockRenewalBehavior>();
}