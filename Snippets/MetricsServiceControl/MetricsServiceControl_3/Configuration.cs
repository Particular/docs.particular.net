namespace MetricsServiceControl_3
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

            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
                interval: TimeSpan.FromSeconds(10),
                instanceId: "INSTANCE_ID_OPTIONAL");
            #endregion
        }
    }
}
