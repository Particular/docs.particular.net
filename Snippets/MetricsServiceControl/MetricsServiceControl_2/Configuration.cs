namespace MetricsServiceControl_2
{
    using NServiceBus;
    using NServiceBus.Metrics.ServiceControl;

    public class Configuration
    {
        public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region SendMetricDataToServiceControl

            endpointConfiguration.SendMetricDataToServiceControl("SERVICE_CONTROL_METRICS_ADDRESS", "INSTANCE_ID_OPTIONAL");

            #endregion
        }
    }
}
