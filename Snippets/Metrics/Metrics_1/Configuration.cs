using System;
using NServiceBus;

namespace Metrics_1
{
    using System.Threading.Tasks;
    using NServiceBus.Logging;

    public class Configuration
    {
        public void ConfigureEndpoint()
        {
            var endpointConfig = new EndpointConfiguration("EndpointName");

            #region Metrics-Enable

            var metricsOptions = endpointConfig.EnableMetrics();

            #endregion

            #region Metrics-Log

            metricsOptions.EnableLogTracing(TimeSpan.FromMinutes(5));

            #endregion

            #region Metrics-Log-Info

            metricsOptions.EnableLogTracing(TimeSpan.FromMinutes(5), LogLevel.Info);

            #endregion

            #region Metrics-Tracing

            metricsOptions.EnableMetricTracing(TimeSpan.FromSeconds(5));

            #endregion

            #region Metrics-Custom-Function

            metricsOptions.EnableCustomReport(data =>
            {
                // process metrics
                return Task.CompletedTask;
            }, TimeSpan.FromSeconds(5));

            #endregion
        }
    }
}
