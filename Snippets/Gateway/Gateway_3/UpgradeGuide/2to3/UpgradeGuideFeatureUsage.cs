namespace Gateway_3.UpgradeGuide._2to3
{
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
}