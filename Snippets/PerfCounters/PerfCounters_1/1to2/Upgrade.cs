namespace PerfCounters
{
    using System;
    using NServiceBus;

    partial class Upgrade
    {

        void UpdateCounterEvery(EndpointConfiguration endpointConfiguration)
        {
            #region 1to2-UpdateCounterEvery

            var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
            // the below line can be removed
            performanceCounters.UpdateCounterEvery(TimeSpan.FromSeconds(2));

            #endregion
        }

    }
}