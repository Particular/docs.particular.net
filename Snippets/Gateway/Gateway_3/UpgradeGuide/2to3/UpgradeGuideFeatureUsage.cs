using NServiceBus;

class UpgradeGuideUsage
{
    void EnableGatewayAfter(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3EnableGatewayAfter

        endpointConfiguration.Gateway();

        #endregion
    }
}
