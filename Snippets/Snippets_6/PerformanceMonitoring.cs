namespace Snippets6
{
    using System;
    using NServiceBus;

    public class PerformanceMonitoring
    {

        public void EnablingCriticalTime()
        {
            #region enable-criticaltime

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.EnableCriticalTimePerformanceCounter();

            #endregion
        }
        public void EnablingSla()
        {
            #region enable-sla

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
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