using System;
using NServiceBus;

class Upgrade1to2
{

    void RegisterObservers(EndpointConfiguration endpointConfiguration)
    {
        #region 1to2RegisterObservers

        var metricsOptions = endpointConfiguration.EnableMetrics();
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