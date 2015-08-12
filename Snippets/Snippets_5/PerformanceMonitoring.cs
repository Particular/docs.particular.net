namespace Snippets5
{
    using System;
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
        public void EnablingSla()
        {
            #region enable-sla

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3));

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