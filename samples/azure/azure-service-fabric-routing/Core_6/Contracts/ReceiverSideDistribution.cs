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
            var localAddress = context.Settings.LogicalAddress();

            context.Pipeline.Register(new ReceiverSideDistributionBehavior(discriminator, discriminators, address => transportInfrastructure.ToTransportAddress(address), localAddress), "Distributes on the receiver side");
        }

        class ReceiverSideDistributionBehavior : IBehavior<IIncomingPhysicalMessageContext, IIncomingPhysicalMessageContext>
        {
            private HashSet<string> discriminators;
            private string discriminator;
            private Func<LogicalAddress, string> addressTranslator;
            private LogicalAddress logicalAddress;

            public ReceiverSideDistributionBehavior(string discriminator, HashSet<string> discriminators, Func<LogicalAddress, string> addressTranslator, LogicalAddress local)
            {
                logicalAddress = local;
                this.addressTranslator = addressTranslator;
                this.discriminator = discriminator;
                this.discriminators = discriminators;
            }

            public async Task Invoke(IIncomingPhysicalMessageContext context, Func<IIncomingPhysicalMessageContext, Task> next)
            {
                string partitionKey;


                if (context.MessageHeaders.TryGetValue(PartitionHeaders.PartitionKey, out partitionKey))
                {
                    Debug.WriteLine($"##### Received message: {context.Message.Headers[Headers.EnclosedMessageTypes]} with Header.PartitionKey={partitionKey} on partition {discriminator}");

                    if (partitionKey == discriminator)
                    {
                        await next(context).ConfigureAwait(false);
                        return;
                    }

                    if (discriminators.Contains(partitionKey))
                    {
                        var destination = addressTranslator(logicalAddress.CreateIndividualizedAddress(partitionKey));
                        await context.ForwardCurrentMessageTo(destination).ConfigureAwait(false);
                        return;
                    }
                }
                // forward to error
                throw new Exception("Will be replaced by unrecoverable exception part of Core PR 4479");
            }
        }
    }
}