namespace Snippets6
{
    using System;
    using NServiceBus;

    public class PerformanceMonitoring
    {

        public void EnablingCriticalTime()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region enable-criticaltime

            endpointConfiguration.EnableCriticalTimePerformanceCounter();

            #endregion
        }
        public void EnablingSla()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
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