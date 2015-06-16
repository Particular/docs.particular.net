namespace Snippets4
{
    using System;
    using NServiceBus;

    public class PerformanceMonitoring
    {
        public void Simple()
        {
            #region PerformanceMonitoring

            Configure.With()
                .EnablePerformanceCounters();

            Configure.With()
                .SetEndpointSLA(TimeSpan.FromMinutes(3));

            #endregion
        }

    }
}