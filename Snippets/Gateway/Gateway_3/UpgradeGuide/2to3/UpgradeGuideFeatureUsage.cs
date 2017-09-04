using NServiceBus;

class UpgradeGuideFeatureUsage
{
    void EnableGatewayAfter(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3EnableGatewayAfter

        endpointConfiguration.Gateway();

        #endregion
    }
}
