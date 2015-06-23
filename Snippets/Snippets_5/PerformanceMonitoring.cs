namespace Snippets5
{
    using System;
    using NServiceBus;

    public class PerformanceMonitoring
    {
        public void V5Upgrade()
        {
            #region PerformanceMonitoring-v5-upgrade-guide

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EnableSLAPerformanceCounter();
            //or
            busConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3));

            #endregion
        }

    }
}