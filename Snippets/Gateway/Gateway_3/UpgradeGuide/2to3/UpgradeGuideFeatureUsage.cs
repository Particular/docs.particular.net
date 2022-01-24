using NServiceBus;
using NServiceBus.Gateway;

class UpgradeGuideUsage
{
    void EnableGatewayAfter(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3EnableGatewayAfter

        endpointConfiguration.Gateway(new InMemoryDeduplicationConfiguration());

        #endregion
    }
}
