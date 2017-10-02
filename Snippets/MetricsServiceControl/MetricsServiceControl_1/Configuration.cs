namespace MetricsServiceControl_1
{
    using NServiceBus;
    using NServiceBus.Metrics.ServiceControl;

    class Configuration
    {
        void ConfigureEndpoint(BusConfiguration busConfiguration)
        {
            #region SendMetricDataToServiceControl

            busConfiguration.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "address",
                instanceId: "INSTANCE_ID_OPTIONAL");

            #endregion
        }
    }
}