using NServiceBus;

class Usage
{
    void TopologySelectionUpgradeGuide(EndpointConfiguration endpointConfiguration)
    {
        // TODO: once ASB v8 is there, the project needs to reference v8.
        // This snippet would need to be commented out.

#pragma warning disable 618
        #region 7to8-number-of-entities-bundle

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var forwardingTopology = transport.UseForwardingTopology();

        forwardingTopology.BundlePrefix("my-prefix");
        forwardingTopology.NumberOfEntitiesInBundle(3);
        
        #endregion
#pragma warning restore 618
    }
}