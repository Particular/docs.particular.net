using System;
using NServiceBus;

public class PerformanceMonitoring
{
    public void Simple()
    {
        #region PerformanceMonitoringV5

        Configure.With(b => b.EnableSLAPerformanceCounter());

        Configure.With(b => b.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3)));

        #endregion
    }

}