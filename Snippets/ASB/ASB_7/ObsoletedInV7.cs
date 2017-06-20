using NServiceBus;

class ObsoletedInV7
{
    public void ObsoletedForwardingTopologySettings(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable 618

        #region 7to8-number-of-entities-bundle [7.0,7.2.3]

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var forwardingTopology = transport.UseForwardingTopology();

        forwardingTopology.BundlePrefix("my-prefix");
        forwardingTopology.NumberOfEntitiesInBundle(3);

        #endregion

#pragma warning restore 618
    }
}