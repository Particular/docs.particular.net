namespace Metrics_1
{
    using System;
    using NServiceBus;

    class Configuration
    {
        void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region SendMetricDataToServiceControl

            var metrics = endpointConfiguration.EnableMetrics();
#pragma warning disable 618
            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "address",
                interval: TimeSpan.FromSeconds(5));
#pragma warning restore 618

            #endregion
        }
    }
}