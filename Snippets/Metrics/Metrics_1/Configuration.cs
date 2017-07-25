#pragma warning disable 618
namespace Metrics_1
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    public class Configuration
    {
        public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region Metrics-Enable

            var metricsOptions = endpointConfiguration.EnableMetrics();

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

            metricsOptions.EnableCustomReport(
                func: data =>
                {
                    // process metrics
                    return Task.CompletedTask;
                },
                interval: TimeSpan.FromSeconds(5));

            #endregion
        }
    }
}