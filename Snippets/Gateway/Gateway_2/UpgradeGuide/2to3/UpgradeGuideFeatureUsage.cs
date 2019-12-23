using NServiceBus;
using NServiceBus.Features;

class UpgradeGuideUsage
{
    void EnableGatewayBefore(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3EnableGatewayBefore

        endpointConfiguration.EnableFeature<Gateway>();

        #endregion
    }
}
