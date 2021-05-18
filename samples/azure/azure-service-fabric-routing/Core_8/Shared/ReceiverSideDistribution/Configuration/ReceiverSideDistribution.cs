using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

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
            pipeline.Register(new DistributeSubscriptions.Register(discriminator, configuration.Partitions, address => transportInfrastructure.ToTransportAddress(address), queueAddress));
        }

        var forwarder = new Forwarder(configuration.Partitions, address => transportInfrastructure.ToTransportAddress(address), queueAddress);
        pipeline.Register(new DistributeMessagesBasedOnHeader(discriminator, forwarder), "Distributes on the receiver side using header only");
        pipeline.Register(new DistributeMessagesBasedOnPayload(discriminator, forwarder, configuration.MapMessageToPartitionKey), "Distributes on the receiver side using user supplied mapper");
    }
}