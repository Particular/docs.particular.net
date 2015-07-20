namespace Snippets4
{
    using System;
    using NServiceBus;

    public class PerformanceMonitoring
    {
        public void EnablingCriticalTime()
        {
            #region enable-criticaltime
            Configure configure = Configure.With();
            // in this version there was no granular control over individual counters
            configure.EnablePerformanceCounters();
            #endregion
        }

        public void EnablingSla()
        {
            #region enable-sla
            Configure configure = Configure.With();
            // in this version there was no granular control over individual counters
            configure.EnablePerformanceCounters();
            configure.SetEndpointSLA(TimeSpan.FromMinutes(3));
            #endregion
        }
        #region enable-sla-host-attribute

        [EndpointSLA("00:03:00")]
        public class EndpointConfig : IConfigureThisEndpoint
        {
        #endregion

            public void Customize(Configure configure)
            {
            }
        }
    }
}