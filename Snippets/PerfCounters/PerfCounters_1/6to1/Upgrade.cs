namespace PerfCounters
{
    using System;
    using NServiceBus;

    partial class Upgrade
    {

        void EnablingCriticalTime(EndpointConfiguration endpointConfiguration)
        {
            #region 6to1-enable-criticaltime

            var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();

            #endregion
        }
        void EnablingSla(EndpointConfiguration endpointConfiguration)
        {
            #region 6to1-enable-sla

            var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
            performanceCounters.EnableSLAPerformanceCounters(TimeSpan.FromMinutes(3));

            #endregion
        }


    }
}