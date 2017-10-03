namespace Core_6
{
    using System;
    using NServiceBus;

    class EndpointBuilder
    {
        public EndpointConfiguration BuildEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("MonitoredEndpoint");

            #region enable-metrics

            var metrics = endpointConfiguration.EnableMetrics();

            #endregion

            #region send-metric-data-to-servicecontrol

            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "Particular.Monitoring", 
                interval: TimeSpan.FromSeconds(10)
            );

            #endregion

            return endpointConfiguration;
        }
    }
}
