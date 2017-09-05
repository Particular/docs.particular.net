namespace Metrics_1
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

            metricsOptions.RegisterObservers(context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register((ref DurationEvent durationEvent) =>
                    {
                        Console.WriteLine($"Duration '{duration.Name}' value observed: '{durationEvent.Duration}'");
                    });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register((ref SignalEvent signalEvent) =>
                    {
                        Console.WriteLine($"Signal '{signal.Name}' value observed: '{signalEvent.MessageType}'");
                    });
                }
            });

            #endregion
        }
    }
}