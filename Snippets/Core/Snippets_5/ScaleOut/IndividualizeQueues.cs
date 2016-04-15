namespace Snippets5.ScaleOut
{
    using NServiceBus;

    class IndividualizeQueues
    {
        IndividualizeQueues(BusConfiguration busConfiguration)
        {
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
