using System;
using NServiceBus;

class CodeFirst
{
    public static void CreateConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region PlatformConnector-CodeFirst
        var platformConnection = new ServicePlatformConnectionConfiguration
        {
            ErrorQueue = "myErrorQueue",
            Heartbeats = new ServicePlatformHeartbeatConfiguration
            {
                Enabled = true,
                HeartbeatsQueue = "heartbeatsQueue"
            },
            CustomChecks = new ServicePlatformCustomChecksConfiguration
            {
                Enabled = true,
                CustomChecksQueue = "customChecksQueue"
            },
            MessageAudit = new ServicePlatformMessageAuditConfiguration
            {
                Enabled = true,
                AuditQueue = "myAuditQueue"
            },
            SagaAudit = new ServicePlatformSagaAuditConfiguration
            {
                Enabled = true,
                SagaAuditQueue = "sagaAuditQueue"
            },
            Metrics = new ServicePlatformMetricsConfiguration
            {
                Enabled = true,
                MetricsQueue = "metricsQueue",
                Interval = TimeSpan.FromSeconds(60)
            }
        };

        endpointConfiguration.ConnectToServicePlatform(platformConnection);
        #endregion
    }
}