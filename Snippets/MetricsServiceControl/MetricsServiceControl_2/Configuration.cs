namespace MetricsServiceControl_2
{
    using NServiceBus;
    using NServiceBus.Metrics.ServiceControl;

    class Configuration
    {
        void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region SendMetricDataToServiceControl

            endpointConfiguration.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "address",
                instanceId: "INSTANCE_ID_OPTIONAL");

            #endregion
        }
    }
}
