using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

class LockRenewalBehavior : Behavior<ITransportReceiveContext>
{
    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        #region native-message-access

        var message = context.Extensions.Get<ServiceBusReceivedMessage>();

        #endregion

        #region get-connection-and-path

        var messageReceiver = context.Extensions.Get<ServiceBusReceiver>();

        #endregion

        var now = DateTimeOffset.UtcNow;
        var remaining = message.LockedUntil - now;

        if (remaining < MaximumRenewBufferDuration)
        {
            throw new Exception($"Not processing message because remaining lock duration is below maximum renew buffer duration ({MaximumRenewBufferDuration}). Consider lowering the prefetch count.")
            {
                Data = { { "ServiceBusReceivedMessage.MessageId", message.MessageId } }
            };
        }

        using (var cts = new CancellationTokenSource())
        {
            var token = cts.Token;

            _ = RenewLockToken(token);

            #region processing-and-cancellation

            try
            {
                await next();
            }
            finally
            {
                remaining = message.LockedUntil - DateTimeOffset.UtcNow;

                if (remaining < MaximumRenewBufferDuration)
                {
                    Log.Warn($"{message.MessageId}: Processing completed but LockedUntil {message.LockedUntil:s}Z less than {MaximumRenewBufferDuration}. This could indicate issues during lock renewal.");
                }

                cts.Cancel();
            }

            #endregion
        }

        #region renewal-background-task

        async Task RenewLockToken(CancellationToken cancellationToken)
        {
            try
            {
                int attempts = 0;

                while (!cancellationToken.IsCancellationRequested)
                {
                    now = DateTimeOffset.UtcNow;
                    remaining = message.LockedUntil - now;

                    var buffer = TimeSpan.FromTicks(Math.Min(remaining.Ticks / 2, MaximumRenewBufferDuration.Ticks));
                    var renewAfter = remaining - buffer;

                    await Task.Delay(renewAfter, cancellationToken);

                    try
                    {
                        await messageReceiver.RenewMessageLockAsync(message, cancellationToken);
                        attempts = 0;
                        Log.Info($"{message.MessageId}: Lock renewed until {message.LockedUntil:s}Z.");
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

    static readonly ILog Log = LogManager.GetLogger<LockRenewalBehavior>();

    // https://github.com/Azure/azure-sdk-for-net/blob/4162f6fa2445b2127468b9cfd080f01c9da88eba/sdk/servicebus/Microsoft.Azure.ServiceBus/src/MessagingUtilities.cs#L10-L23
    static readonly TimeSpan MaximumRenewBufferDuration = TimeSpan.FromSeconds(10);
}