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

        var durationsLog = LogManager.GetLogger("Durations");
        var signalsLog = LogManager.GetLogger("Signals");

        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: (ref DurationEvent @event) =>
                        {
                            durationsLog.Debug($"{duration.Name} = {@event.Duration}");
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: (ref SignalEvent @event) =>
                        {
                            signalsLog.Debug($"{signal.Name} = {@event.MessageType}");
                        });
                }
            });

        #endregion
    }
}
