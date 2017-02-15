using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

namespace Contracts
{
    class ReceiverSideDistribution : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var discriminators = context.Settings.Get<HashSet<string>>("ReceiverSideDistribution.Discriminators");
            var mapper = context.Settings.Get<Func<object,string>>("ReceiverSideDistribution.Mapper");
            var discriminator = context.Settings.Get<string>("EndpointInstanceDiscriminator");
            var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();
            var logicalAddress = context.Settings.LogicalAddress();

            context.Pipeline.Register(new PhysicalMessageReceiverSideDistributionBehavior(discriminator, discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress), "Distributes on the receiver side using header only");
            context.Pipeline.Register(new LogicalMessageReceiverSideDistributionBehavior(discriminator, discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress, mapper), "Distributes on the receiver side using user supplied mapper");
        }

        class PhysicalMessageReceiverSideDistributionBehavior : IBehavior<IIncomingPhysicalMessageContext, IIncomingPhysicalMessageContext>
        {
            private readonly HashSet<string> knownPartitionKeys;
            private readonly string localPartitionKey;
            private readonly Func<LogicalAddress, string> addressTranslator;
            private LogicalAddress logicalAddress;

            public PhysicalMessageReceiverSideDistributionBehavior(string localPartitionKey, HashSet<string> knownPartitionKeys, Func<LogicalAddress, string> addressTranslator, LogicalAddress logicalAddress)
            {
                this.logicalAddress = logicalAddress;
                this.addressTranslator = addressTranslator;
                this.localPartitionKey = localPartitionKey;
                this.knownPartitionKeys = knownPartitionKeys;
            }

            public async Task Invoke(IIncomingPhysicalMessageContext context, Func<IIncomingPhysicalMessageContext, Task> next)
            {
                var intent = context.Message.GetMesssageIntent();
                var isPubSubRelatedMessage = intent == MessageIntentEnum.Subscribe || intent == MessageIntentEnum.Unsubscribe;


                if (isPubSubRelatedMessage)
                {
                    var tasks = new List<Task>
                    {
                        next(context)
                    };

                    //Check to see if subscription message was already forwarded to prevent infinite loop
                    if (!context.MessageHeaders.ContainsKey(PartitionHeaders.ForwardedSubscription))
                    {
                        context.Message.Headers[PartitionHeaders.ForwardedSubscription] = string.Empty;

                        foreach (var partitionKey in knownPartitionKeys.ToList().Where(key => key != localPartitionKey))
                        {
                            var destination = addressTranslator(logicalAddress.CreateIndividualizedAddress(partitionKey));
                            tasks.Add(context.ForwardCurrentMessageTo(destination));
                        }
                    }

                    await Task.WhenAll(tasks).ConfigureAwait(false);
                    return;
                }

                string messagePartitionKey;
                var hasPartitionKeyHeader = context.MessageHeaders.TryGetValue(PartitionHeaders.PartitionKey, out messagePartitionKey);

                //1. The intent is subscribe or unsubscribe
                //2. The header value isn't present (logical behavior will check message contents)
                //3. The header value matches local partition key
                if (isPubSubRelatedMessage || !hasPartitionKeyHeader || messagePartitionKey == localPartitionKey)
                {
                    await next(context).ConfigureAwait(false); //Continue the pipeline as usual
                    return;
                }



                if (knownPartitionKeys.Contains(messagePartitionKey)) //Forward message to known instance
                {
                    var destination = addressTranslator(logicalAddress.CreateIndividualizedAddress(messagePartitionKey));
                    await context.ForwardCurrentMessageTo(destination).ConfigureAwait(false);
                    return;
                }

                throw new Exception($"Header key value {messagePartitionKey} does not map to any known partition key values."); //Will be replaced by unrecoverable exception part of Core PR 4479
            }
        }

        class LogicalMessageReceiverSideDistributionBehavior : IBehavior<IIncomingLogicalMessageContext, IIncomingLogicalMessageContext>
        {
            private readonly string localPartitionKey;
            private readonly HashSet<string> knownPartitionKeys;
            private readonly Func<LogicalAddress, string> addressTranslator;
            private readonly LogicalAddress logicalAddress;
            private readonly Func<object, string> mapper;

            public LogicalMessageReceiverSideDistributionBehavior(string localPartitionKey, HashSet<string> knownPartitionKeys, Func<LogicalAddress, string> addressTranslator, LogicalAddress logicalAddress, Func<object, string> mapper)
            {
                this.localPartitionKey = localPartitionKey;
                this.knownPartitionKeys = knownPartitionKeys;
                this.addressTranslator = addressTranslator;
                this.logicalAddress = logicalAddress;
                this.mapper = mapper;
            }

            public async Task Invoke(IIncomingLogicalMessageContext context, Func<IIncomingLogicalMessageContext, Task> next)
            {
                string messagePartitionKey;

                if (!context.MessageHeaders.TryGetValue(PartitionHeaders.PartitionKey, out messagePartitionKey))
                {
                    messagePartitionKey = mapper(context.Message.Instance);

                    if (string.IsNullOrWhiteSpace(messagePartitionKey))
                    {
                        throw new Exception($"Could not map a partition key for message of type {context.Headers[Headers.EnclosedMessageTypes]}"); //Will be replaced by unrecoverable exception part of Core PR 4479
                    }
                }

                Debug.WriteLine($"##### Received message: {context.Headers[Headers.EnclosedMessageTypes]} with Mapped PartitionKey={messagePartitionKey} on partition {localPartitionKey}");

                if (messagePartitionKey == localPartitionKey)
                {
                    await next(context).ConfigureAwait(false); //Continue the pipeline as usual
                    return;
                }

                if (!knownPartitionKeys.Contains(messagePartitionKey))
                {
                    throw new Exception($"User mapped key {messagePartitionKey} does not match any known partition key values"); //Will be replaced by unrecoverable exception part of Core PR 4479
                }

                var destination = addressTranslator(logicalAddress.CreateIndividualizedAddress(messagePartitionKey));
                await context.ForwardCurrentMessageTo(destination).ConfigureAwait(false);
            }
        }
    }
}