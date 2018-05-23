using System;
using NServiceBus;

#pragma warning disable 618
public class Configuration
{
    public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
    {
        var metrics = endpointConfiguration.EnableMetrics();

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