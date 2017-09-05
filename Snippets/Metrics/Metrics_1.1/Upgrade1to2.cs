using System.Diagnostics;
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
                duration.Register(durationLength =>
                {
                    Trace.WriteLine($"Duration '{duration.Name}' value observed: '{durationLength}'");
                });
            }
            foreach (var signal in context.Signals)
            {
                signal.Register(() =>
                {
                    Trace.WriteLine($"'{signal.Name}' occurred.");
                });
            }
        });
        #endregion
    }

}