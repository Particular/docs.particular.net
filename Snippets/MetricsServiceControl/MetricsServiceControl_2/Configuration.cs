namespace MetricsServiceControl_2
{
    using NServiceBus;
    using NServiceBus.Metrics.ServiceControl;

    class Configuration
    {
        void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region SendMetricDataToServiceControl

            const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

            endpointConfiguration.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
                instanceId: "INSTANCE_ID_OPTIONAL");

            #endregion
        }
    }
}
