namespace Core8.Tutorials.SetupMonitoring
{
    using NServiceBus;
    using System;

    static class Configuration
    {
        static void Configure(EndpointConfiguration endpointConfiguration)
        {
            #region SetupMonitoring-ConfigureError
            endpointConfiguration.SendFailedMessagesTo("error");
            #endregion

            #region SetupMonitoring-ConfigureAudit
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            #endregion

            #region SetupMonitoring-ConfigureMetrics
            var metrics = endpointConfiguration.EnableMetrics();

            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "Particular.Monitoring",
                interval: TimeSpan.FromSeconds(2)
            );
            #endregion
        }
    }
}
