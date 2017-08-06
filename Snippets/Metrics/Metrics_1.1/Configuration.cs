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

            metricsOptions.EnableLogTracing(interval: TimeSpan.FromMinutes(5));

            #endregion

            #region Metrics-Log-Info

            metricsOptions.EnableLogTracing(
                interval: TimeSpan.FromMinutes(5),
                logLevel: LogLevel.Info);

            #endregion

            #region Metrics-Tracing

            metricsOptions.EnableMetricTracing(interval: TimeSpan.FromSeconds(5));

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

            #region Metrics-Observers

            metricsOptions.RegisterObservers(context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(durationLength =>
                    {
                        Console.WriteLine($"Duration '{duration.Name}' value observed: '{durationLength}'");
                    });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(() =>
                    {
                        Console.WriteLine($"'{signal.Name}' occurred.");
                    });
                }
            });

            #endregion
        }
    }
}