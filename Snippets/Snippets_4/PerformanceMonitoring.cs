namespace Snippets4
{
    using System;
    using NServiceBus;

    public class PerformanceMonitoring
    {
        public void V5Upgrade()
        {
            #region PerformanceMonitoring-v5-upgrade-guide

            Configure.With()
                .EnablePerformanceCounters();

            Configure.With()
                .SetEndpointSLA(TimeSpan.FromMinutes(3));

            #endregion
        }

    }
}