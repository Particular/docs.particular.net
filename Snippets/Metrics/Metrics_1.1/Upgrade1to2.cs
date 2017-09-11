using System.Diagnostics;
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
                        observer: length =>
                        {
                            log.Info($"Duration: '{duration.Name}'. Value: '{length}'");
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: () =>
                        {
                            log.Info($"signal: '{signal.Name}'");
                        });
                }
            });

        #endregion
    }

}