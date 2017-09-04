namespace Gateway_3.UpgradeGuide._2to3
{
    using NServiceBus;
    using NServiceBus.Features;

    class Usage
    {
        void NewWayToEnable(EndpointConfiguration endpointConfiguration)
        {
            #region 2to3NewWayToEnable

            endpointConfiguration.Gateway();

            #endregion
        }
        void OldWayToEnable(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable 0618
            #region 2to3OldWayToEnable

            endpointConfiguration.EnableFeature<Gateway>();

            #endregion
#pragma warning restore 0618
        }
    }
}