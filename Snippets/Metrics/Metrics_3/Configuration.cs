using System;
using NServiceBus;

public class Configuration
{
    public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region Metrics-Enable

        var metrics = endpointConfiguration.EnableMetrics();

        #endregion

        #region Metrics-Observers

        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: (ref DurationEvent @event) =>
                        {
                            Console.WriteLine($"Duration: '{duration.Name}'. Value: '{@event.Duration}'");
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: (ref SignalEvent @event) =>
                        {
                            Console.WriteLine($"Signal: '{signal.Name}'. Type: '{@event.MessageType}'");
                        });
                }
            });

        #endregion
    }
}