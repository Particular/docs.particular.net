namespace Snippets_5.ScaleOut
{
    using NServiceBus;

    class individualize_queues
    {
        public void ConfigurePropertyInjectionForHandler()
        {
            BusConfiguration configuration = new BusConfiguration();

            #region UniqueQueuePerEndpointInstance 5.2

            configuration.ScaleOut().UniqueQueuePerEndpointInstance();

            #endregion

            #region UniqueQueuePerEndpointInstanceWithSuffix 5.2

            configuration.ScaleOut().UniqueQueuePerEndpointInstance("-MyCustomSuffix");

            #endregion
        }
    }
}
