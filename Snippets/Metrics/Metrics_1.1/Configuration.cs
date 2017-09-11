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

            var metrics = endpointConfiguration.EnableMetrics();

            #endregion

            #region Metrics-Log

            metrics.EnableLogTracing(interval: TimeSpan.FromMinutes(5));

            #endregion

            #region Metrics-Log-Info

            metrics.EnableLogTracing(
                interval: TimeSpan.FromMinutes(5),
                logLevel: LogLevel.Info);

            #endregion

            #region Metrics-Tracing

            metrics.EnableMetricTracing(interval: TimeSpan.FromSeconds(5));

            #endregion

            #region Metrics-Custom-Function

            metrics.EnableCustomReport(
                func: data =>
                {
                    // process metrics
                    return Task.CompletedTask;
                },
                interval: TimeSpan.FromSeconds(5));

            #endregion

            #region Metrics-Observers

            metrics.RegisterObservers(
                register: context =>
                {
                    foreach (var duration in context.Durations)
                    {
                        duration.Register(
                            observer: length =>
                            {
                                Console.WriteLine($"Duration: '{duration.Name}'. Value: '{length}'");
                            });
                    }
                    foreach (var signal in context.Signals)
                    {
                        signal.Register(
                            observer: () =>
                            {
                                Console.WriteLine($"Signal: '{signal.Name}'");
                            });
                    }
                });

            #endregion
        }
    }
}