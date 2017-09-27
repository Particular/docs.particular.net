namespace MetricsServiceControl_1
{
    using NServiceBus;
    using NServiceBus.Metrics.ServiceControl;

    public class Configuration
    {
        public void ConfigureEndpoint(BusConfiguration busConfiguration)
        {
            #region SendMetricDataToServiceControl

            busConfiguration.SendMetricDataToServiceControl("SERVICE_CONTROL_METRICS_ADDRESS", "INSTANCE_ID_OPTIONAL");

            #endregion
        }
    }
}
