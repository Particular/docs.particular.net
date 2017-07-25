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

            #region Metrics-Observers

            metricsOptions.RegisterObservers(ctx =>
            {
                foreach (var duration in ctx.Durations)
                {
                    duration.Register(durationLength =>
                    {
                        Console.WriteLine($"Duration '{duration.Name}' value observed: '{durationLength}'");
                    });
                }
                foreach (var signal in ctx.Signals)
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