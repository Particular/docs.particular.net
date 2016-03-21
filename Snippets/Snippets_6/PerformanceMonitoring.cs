namespace Snippets6
{
    using System;
    using NServiceBus;

    class PerformanceMonitoring
    {

        void EnablingCriticalTime(EndpointConfiguration endpointConfiguration)
        {
            #region enable-criticaltime

            endpointConfiguration.EnableCriticalTimePerformanceCounter();

            #endregion
        }
        void EnablingSla(EndpointConfiguration endpointConfiguration)
        {
            #region enable-sla

            endpointConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3));

            #endregion
        }

        #region enable-sla-host-attribute

        [EndpointSLA("00:03:00")]
        public class EndpointConfig : IConfigureThisEndpoint
        {
            #endregion

            public void Customize(EndpointConfiguration endpointConfiguration)
            {
            }
        }

    }
}