using System;
using NServiceBus;

namespace Metrics_1
{
    public class Configuration
    {
        public void ConfigureEndpoint()
        {
            var endpointConfig = new EndpointConfiguration("EndpointName");

            #region Metrics-Enable

            var metricsOptions = endpointConfig.EnableMetrics();

            #endregion

            #region Metrics-DefaultInterval

            metricsOptions.SetDefaultInterval(TimeSpan.FromSeconds(45));

            #endregion

            #region Metrics-Tracing

            metricsOptions.EnableMetricTracing(TimeSpan.FromSeconds(5));

            #endregion

            #region Metrics-ServiceControl

            metricsOptions.SendMetricDataToServiceControl(
                "serviceControl-metrics-address",
                TimeSpan.FromSeconds(50)
            );

            #endregion
        }
    }
}
