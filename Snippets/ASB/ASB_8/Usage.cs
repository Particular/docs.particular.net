using NServiceBus;

class Usage
{
    void TopologySelectionUpgradeGuide(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8-number-of-entities-bundle

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        transport.UseForwardingTopology();

        // TODO

        #endregion
    }
}