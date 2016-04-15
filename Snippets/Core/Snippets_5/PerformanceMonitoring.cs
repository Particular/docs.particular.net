namespace Core5
{
    using System;
    using NServiceBus;

    class PerformanceMonitoring
    {

        void EnablingCriticalTime(BusConfiguration busConfiguration)
        {
            #region enable-criticaltime

            busConfiguration.EnableCriticalTimePerformanceCounter();

            #endregion
        }
        void EnablingSla(BusConfiguration busConfiguration)
        {
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