namespace Snippets_5.ScaleOut
{
    using NServiceBus;

    class individualize_queues
    {
        public void ConfigurePropertyInjectionForHandler()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region UniqueQueuePerEndpointInstance 5.2

            busConfiguration.ScaleOut().UniqueQueuePerEndpointInstance();

            #endregion

            #region UniqueQueuePerEndpointInstanceWithSuffix 5.2

            busConfiguration.ScaleOut().UniqueQueuePerEndpointInstance("-MyCustomSuffix");

            #endregion
        }
    }
}
