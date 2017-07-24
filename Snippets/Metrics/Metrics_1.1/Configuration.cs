namespace Metrics_1_1
{
    using System;
    using NServiceBus;

    public class Configuration
    {
        public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region Metrics-Enable

            var metricsOptions = endpointConfiguration.EnableMetrics();

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