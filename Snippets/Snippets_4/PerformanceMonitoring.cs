﻿using System;
using NServiceBus;

public class PerformanceMonitoring
{
    public void Simple()
    {
        #region PerformanceMonitoringV4

        Configure.With()
            .EnablePerformanceCounters();

        Configure.With()
            .SetEndpointSLA(TimeSpan.FromMinutes(3));

        #endregion
    }

}