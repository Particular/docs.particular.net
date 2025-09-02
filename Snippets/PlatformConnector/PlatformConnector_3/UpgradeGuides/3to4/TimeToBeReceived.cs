using System;
using NServiceBus;

public class TimeToBeReceived
{
    public static void Usage(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region PlatformConnector-3to4-TimeToBeReceived
        var platformConnection = new ServicePlatformConnectionConfiguration
        {
            Metrics = new ServicePlatformMetricsConfiguration
            {
                TimeToBeReceived = TimeSpan.FromSeconds(60)
            }
        };

        endpointConfiguration.ConnectToServicePlatform(platformConnection);
        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }
}