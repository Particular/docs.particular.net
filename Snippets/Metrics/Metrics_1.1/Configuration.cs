using NServiceBus;
using NServiceBus.Logging;

#pragma warning disable 618
public class Configuration
{
    public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
    {
        var metrics = endpointConfiguration.EnableMetrics();

        #region Metrics-Observers

        var durationsLog = LogManager.GetLogger("Durations");
        var signalsLog = LogManager.GetLogger("Signals");

        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: length =>
                        {
                            durationsLog.DebugFormat("{0} = {1}", duration.Name, length);
                        });
                }
                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: () =>
                        {
                            signalsLog.DebugFormat("{0}", signal.Name);
                        });
                }
            });

        #endregion
    }
}