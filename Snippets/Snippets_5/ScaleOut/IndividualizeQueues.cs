namespace Snippets5.ScaleOut
{
    using NServiceBus;

    class IndividualizeQueues
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region UniqueQueuePerEndpointInstance 5.2

            busConfiguration.ScaleOut()
                .UniqueQueuePerEndpointInstance();

            #endregion

            #region UniqueQueuePerEndpointInstanceWithSuffix 5.2

            busConfiguration.ScaleOut()
                .UniqueQueuePerEndpointInstance("-MyCustomSuffix");

            #endregion
        }
    }
}
