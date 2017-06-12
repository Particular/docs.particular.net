using NServiceBus;

class Usage
{
    void TopologySelectionUpgradeGuide(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8-number-of-entities-bundle

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var forwardingTopology = transport.UseForwardingTopology();

        // forwardingTopology.BundlePrefix("my-prefix");
        // forwardingTopology.NumberOfEntitiesInBundle(3);
        
        #endregion
    }
}