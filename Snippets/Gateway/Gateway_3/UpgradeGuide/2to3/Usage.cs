namespace Gateway_3.UpgradeGuide._2to3
{
    using NServiceBus;
    using NServiceBus.Features;

    class Usage
    {
        void EnableGatewayAfter(EndpointConfiguration endpointConfiguration)
        {
            #region 2to3EnableGatewayAfter

            endpointConfiguration.Gateway();

            #endregion
        }
        void EnableGatewayBefore(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable 0618
            #region 2to3EnableGatewayBefore

            endpointConfiguration.EnableFeature<Gateway>();

            #endregion
#pragma warning restore 0618
        }
    }
}