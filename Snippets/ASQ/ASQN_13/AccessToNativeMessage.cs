using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using Azure.Storage.Queues.Models;

namespace ASQN_13
{
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
}
