namespace Core5.ScaleOut
{
    using NServiceBus;

    class IndividualizeQueues
    {
        void UniqueQueuePerEndpointInstance(BusConfiguration busConfiguration)
        {
            #region UniqueQueuePerEndpointInstance

            var scaleOutSettings = busConfiguration.ScaleOut();
            scaleOutSettings.UniqueQueuePerEndpointInstance();

            #endregion
        }

        void UniqueQueuePerEndpointInstanceWithSuffix(BusConfiguration busConfiguration)
        {
            #region UniqueQueuePerEndpointInstanceWithSuffix

            var scaleOutSettings = busConfiguration.ScaleOut();
            scaleOutSettings.UniqueQueuePerEndpointInstance("-MyCustomSuffix");

            #endregion
        }
    }
}
