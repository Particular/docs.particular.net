namespace PerfCounters
{
    using System;
    using NServiceBus;

    class PerformanceMonitoring
    {
        void EnablingCriticalTime(EndpointConfiguration endpointConfiguration)
        {
            #region enable-criticaltime

            var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();

            #endregion

            #region update-counters-every

            performanceCounters.UpdateCounterEvery(TimeSpan.FromSeconds(2));

            #endregion
        }
        void EnablingSla(EndpointConfiguration endpointConfiguration)
        {
            #region enable-sla

            var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
            performanceCounters.EnableSLAPerformanceCounters(TimeSpan.FromMinutes(3));

            #endregion
        }
    }
}