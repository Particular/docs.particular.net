namespace PerfCounters
{
    using System;
    using NServiceBus;

    class PerformanceMonitoring
    {
        void EnablingCriticalTime(EndpointConfiguration endpointConfiguration)
        {
            var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();

            #region update-counters-every

            performanceCounters.UpdateCounterEvery(TimeSpan.FromSeconds(2));

            #endregion
        }
    }
}