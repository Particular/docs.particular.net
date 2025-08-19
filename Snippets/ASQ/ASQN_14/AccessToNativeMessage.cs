using System;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;
using NServiceBus.Pipeline;

public class AccessToNativeMessage
{
    #region access-native-incoming-message

    class DoNotAttemptMessageProcessingIfMessageIsNotLocked : Behavior<ITransportReceiveContext>
    {
        public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            var NextVisibleOnUtc = context.Extensions.Get<QueueMessage>().NextVisibleOn;

            if (NextVisibleOnUtc <= DateTime.UtcNow)
            {
                return next();
            }

            throw new Exception($"Message lock lost for MessageId {context.Message.MessageId} and it cannot be processed.");
        }
    }

    #endregion
}