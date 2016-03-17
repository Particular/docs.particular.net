namespace Snippets5
{
    using System;
    using NServiceBus;

    public class PerformanceMonitoring
    {

        public void EnablingCriticalTime()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region enable-criticaltime

            busConfiguration.EnableCriticalTimePerformanceCounter();

            #endregion
        }
        public void EnablingSla()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region enable-sla

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