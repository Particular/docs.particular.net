using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;
using Microsoft.Extensions.DependencyInjection;

class ReceiverSideDistribution :
    Feature
{
    public ReceiverSideDistribution()
    {
        Defaults(s => s.AddUnrecoverableException(typeof(PartitionMappingFailedException)));
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;
        var configuration = settings.Get<PartitionAwareReceiverSideDistributionConfiguration>();

        var discriminator = settings.Get<string>("EndpointInstanceDiscriminator");
        var transportInfrastructure = settings.Get<TransportDefinition>();
        var logicalAddress = settings.EndpointQueueName();

        var supportMessageDrivenPubSub = transportInfrastructure.SupportsPublishSubscribe;
        var queueAddress = new QueueAddress(logicalAddress, null, null, null);

        var pipeline = context.Pipeline;
        if (supportMessageDrivenPubSub)
        {
            pipeline.Register(new DistributeSubscriptions.Register(discriminator, configuration.Partitions, queueAddress));
        }

        pipeline.Register(b => new DistributeMessagesBasedOnHeader(discriminator, new Forwarder(configuration.Partitions, b.GetRequiredService<ITransportAddressResolver>(), queueAddress)), "Distributes on the receiver side using header only");
        pipeline.Register(b => new DistributeMessagesBasedOnPayload(discriminator, new Forwarder(configuration.Partitions, b.GetRequiredService<ITransportAddressResolver>(), queueAddress), configuration.MapMessageToPartitionKey), "Distributes on the receiver side using user supplied mapper");
    }
}