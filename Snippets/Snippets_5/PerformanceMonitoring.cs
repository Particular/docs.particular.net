namespace Snippets5
{
    using NServiceBus;

    public class PerformanceMonitoring
    {

        public void EnablingCriticalTime()
        {
            #region enable-criticaltime

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EnableCriticalTimePerformanceCounter();

            #endregion
        }

        #region enable-sla-host-attribute

        [EndpointSLA("00:03:00")]
        public class EndpointConfig : IConfigureThisEndpoint
        {
            #endregion

            public void Customize(BusConfiguration busConfiguration)
            {
            }
        }

    }
}