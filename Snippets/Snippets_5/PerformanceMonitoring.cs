using System;
using NServiceBus;

public class PerformanceMonitoring
{
    public void Simple()
    {
        #region PerformanceMonitoring

        var configuration = new BusConfiguration();

        configuration.EnableSLAPerformanceCounter();
        //or
        configuration.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3));

        #endregion
    }

}