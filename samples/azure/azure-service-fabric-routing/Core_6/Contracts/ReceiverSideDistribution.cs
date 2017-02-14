using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var discriminator = context.Settings.Get<string>("EndpointInstanceDiscriminator");
            var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();
            var logicalAddress = context.Settings.LogicalAddress();

            context.Pipeline.Register(new ReceiverSideDistributionBehavior(discriminator, discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress), "Distributes on the receiver side");
        }

        class ReceiverSideDistributionBehavior : IBehavior<IIncomingPhysicalMessageContext, IIncomingPhysicalMessageContext>
        {
            private readonly HashSet<string> knownPartitionKeys;
            private readonly string localPartitionKey;
            private readonly Func<LogicalAddress, string> addressTranslator;
            private LogicalAddress logicalAddress;

            public ReceiverSideDistributionBehavior(string localPartitionKey, HashSet<string> knownPartitionKeys, Func<LogicalAddress, string> addressTranslator, LogicalAddress logicalAddress)
            {
                this.logicalAddress = logicalAddress;
                this.addressTranslator = addressTranslator;
                this.localPartitionKey = localPartitionKey;
                this.knownPartitionKeys = knownPartitionKeys;
            }

            public async Task Invoke(IIncomingPhysicalMessageContext context, Func<IIncomingPhysicalMessageContext, Task> next)
            {
                string messagePartitionKey;

                //Check intent of message to make sure it isn't subscribe or unsubscribe

                if (context.MessageHeaders.TryGetValue(PartitionHeaders.PartitionKey, out messagePartitionKey))
                {
                    Debug.WriteLine($"##### Received message: {context.Message.Headers[Headers.EnclosedMessageTypes]} with Header.PartitionKey={messagePartitionKey} on partition {localPartitionKey}");

                    if (messagePartitionKey == localPartitionKey)
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
                }

                //Try to determine if this belongs to a partition using message mapping function

                // forward to error
                throw new Exception("Will be replaced by unrecoverable exception part of Core PR 4479");
            }
        }
    }
}