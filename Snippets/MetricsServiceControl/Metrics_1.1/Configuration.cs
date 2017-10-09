namespace Metrics_1
{
    using System;
    using NServiceBus;

    class Configuration
    {
        void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region SendMetricDataToServiceControl

            const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

            var metrics = endpointConfiguration.EnableMetrics();
#pragma warning disable 618
            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
                interval: TimeSpan.FromSeconds(5));
#pragma warning restore 618

            #endregion
        }
    }
}