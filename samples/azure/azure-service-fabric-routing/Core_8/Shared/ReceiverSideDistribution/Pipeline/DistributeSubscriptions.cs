using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

class DistributeSubscriptions : IBehavior<IIncomingPhysicalMessageContext, IIncomingPhysicalMessageContext>
{
    HashSet<string> knownPartitionKeys;
    string localPartitionKey;
    Func<QueueAddress, string> addressTranslator;
    QueueAddress logicalAddress;

    public DistributeSubscriptions(string localPartitionKey, HashSet<string> knownPartitionKeys, Func<QueueAddress, string> addressTranslator, QueueAddress logicalAddress)
    {
        this.logicalAddress = logicalAddress;
        this.addressTranslator = addressTranslator;
        this.localPartitionKey = localPartitionKey;
        this.knownPartitionKeys = knownPartitionKeys;
    }

    public async Task Invoke(IIncomingPhysicalMessageContext context, Func<IIncomingPhysicalMessageContext, Task> next)
    {
        var intent = context.Message.GetMessageIntent();
        var isSubscriptionMessage = intent == MessageIntentEnum.Subscribe || intent == MessageIntentEnum.Unsubscribe;

        if (isSubscriptionMessage)
        {
            var tasks = new List<Task>();

            // Check to see if subscription message was already forwarded to prevent infinite loop
            if (!context.MessageHeaders.ContainsKey(PartitionHeaders.ForwardedSubscription))
            {
                context.Message.Headers[PartitionHeaders.ForwardedSubscription] = string.Empty;

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var partitionKey in knownPartitionKeys)
                {
                    if (partitionKey == localPartitionKey)
                    {
                        continue;
                    }

                    var queueAddress = new QueueAddress(logicalAddress.BaseAddress, partitionKey, null, null);
                    var destination = addressTranslator(queueAddress);
                    tasks.Add(context.ForwardCurrentMessageTo(destination));
                }
            }
            await Task.WhenAll(tasks)
                .ConfigureAwait(false);
        }
        await next(context)
            .ConfigureAwait(false);
    }

    public class Register :
        RegisterStep
    {
        public Register(string localPartitionKey, HashSet<string> knownPartitionKeys, Func<QueueAddress, string> addressTranslator, QueueAddress logicalAddress) :
            base("DistributeSubscriptions", typeof(DistributeSubscriptions), "Distributes subscription messages for message driven pubsub using header only.", b => new DistributeSubscriptions(localPartitionKey, knownPartitionKeys, addressTranslator, logicalAddress))
        {
            InsertBeforeIfExists("ProcessSubscriptionRequests");
        }
    }
}