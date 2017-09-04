namespace Gateway_2.UpgradeGuide._2to3
{
    using NServiceBus;
    using NServiceBus.Features;

    class UpgradeGuideFeatureUsage
    {
        void EnableGatewayBefore(EndpointConfiguration endpointConfiguration)
        {
            #region 2to3EnableGatewayBefore

            endpointConfiguration.EnableFeature<Gateway>();

            #endregion
        }
    }
}