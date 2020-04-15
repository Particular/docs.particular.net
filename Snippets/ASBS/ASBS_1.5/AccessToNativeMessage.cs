namespace ASBS_1
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using NServiceBus.Pipeline;

    public class AccessToNativeMessage
    {
        #region access-native-message

        class DoNotAttemptMessageProcessingIfMessageIsNotLocked : Behavior<ITransportReceiveContext>
        {
            public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
            {
                var lockedUntilUtc = context.Extensions.Get<Message>().SystemProperties.LockedUntilUtc;

                if (lockedUntilUtc <= DateTime.UtcNow)
                {
                    return next();
                }

                context.AbortReceiveOperation();

                return Task.CompletedTask;
            }
        }

        #endregion
    }
}