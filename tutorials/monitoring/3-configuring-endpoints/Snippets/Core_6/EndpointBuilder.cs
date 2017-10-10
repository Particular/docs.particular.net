namespace Core_6
{
    using System;
    using NServiceBus;

    class EndpointBuilder
    {
        public EndpointConfiguration BuildEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("MonitoredEndpoint");

            #region error-config
            endpointConfiguration.SendFailedMessagesTo("error");
            #endregion

            #region audit-config
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            #endregion

            #region send-metric-data-to-servicecontrol

            var metrics = endpointConfiguration.EnableMetrics();

            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "Particular.Monitoring", 
                interval: TimeSpan.FromSeconds(10)
            );

            #endregion

            return endpointConfiguration;
        }
    }
}
