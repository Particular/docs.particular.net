

using System;
using NServiceBus;

public class TimeToBeReceived
{
    public static void Usage(EndpointConfiguration endpointConfiguration)
    {
        #region PlatformConnector-3to4-TimeToBeReceived
        var platformConnection = new ServicePlatformConnectionConfiguration
        {
            Metrics = new ServicePlatformMetricsConfiguration
            {
                Enabled = true,
                MetricsQueue = "metricsQueue",
                Interval = TimeSpan.FromSeconds(60),
                TimeToLive = TimeSpan.FromSeconds(60)
            }
        };

        endpointConfiguration.ConnectToServicePlatform(platformConnection);
        #endregion
    }
}