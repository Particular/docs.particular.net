using NServiceBus;
using NServiceBus.Logging;

class Upgrade1to2
{

    void RegisterObservers(EndpointConfiguration endpointConfiguration)
    {
        var log = LogManager.GetLogger("RegisterObservers");

        #region 1to2RegisterObservers

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: (ref DurationEvent @event) =>
                        {
                            log.Info($"Duration: '{duration.Name}'. Value: '{@event.Duration}'");
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: (ref SignalEvent @event) =>
                        {
                            log.Info($"Signal: '{signal.Name}'. Type: '{@event.MessageType}'");
                        });
                }
            });

        #endregion
    }

}