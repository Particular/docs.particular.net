namespace Core5.Tutorials.SetupMonitoring
{
    using NServiceBus;
    using NServiceBus.Metrics.ServiceControl;

    static class Configuration
    {
        static void Configure(BusConfiguration busConfiguration)
        {
            #region SetupMonitoring-ConfigureMetrics
            busConfiguration.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "Particular.Monitoring"
            );
            #endregion
        }
    }
}
