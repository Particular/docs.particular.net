using System;
using NServiceBus;

public class PerformanceMonitoring
{
    public void Simple()
    {
        #region PerformanceMonitoring

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EnableSLAPerformanceCounter();
        //or
        busConfiguration.EnableSLAPerformanceCounter(TimeSpan.FromMinutes(3));

        #endregion
    }

}