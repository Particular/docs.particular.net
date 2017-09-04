namespace Gateway_3.UpgradeGuide._2to3
{
    using NServiceBus;
    using NServiceBus.Features;

    class Usage
    {
        void EnableGatewayBefore(EndpointConfiguration endpointConfiguration)
        {
            #region 2to3EnableGatewayBefore

            endpointConfiguration.Gateway();

            #endregion
        }
        void EnableGatewayAfter(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable 0618
            #region 2to3EnableGatewayAfter

            endpointConfiguration.EnableFeature<Gateway>();

            #endregion
#pragma warning restore 0618
        }
    }
}