namespace Snippets6
{
    using System;
    using NServiceBus;

    public class PerformanceMonitoring
    {

        public void EnablingCriticalTime()
        {
            #region enable-criticaltime

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.EnableCriticalTimePerformanceCounter();

            #endregion
        }
        public void EnablingSla()
        {
            #region enable-sla

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3));

            #endregion
        }

        #region enable-sla-host-attribute

        [EndpointSLA("00:03:00")]
        public class EndpointConfig : IConfigureThisEndpoint
        {
            #endregion

            public void Customize(EndpointConfiguration configuration)
            {
            }
        }

    }
}