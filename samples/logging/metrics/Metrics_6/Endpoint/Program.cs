using System;
using System.Diagnostics;
using NServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

Console.Title = "TracingEndpoint";

var host = Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .UseNServiceBus(_ =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.Metrics.Tracing.Endpoint");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region EnableMetricTracing

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.RegisterObservers(
            register: context =>
            {
                foreach (var duration in context.Durations)
                {
                    duration.Register(
                        observer: (ref DurationEvent @event) =>
                        {
                            Trace.WriteLine(
                                $"Duration: '{duration.Name}'. Value: '{@event.Duration}' ({@event.MessageType})");
                        });
                }

                foreach (var signal in context.Signals)
                {
                    signal.Register(
                        observer: (ref SignalEvent @event) =>
                        {
                            Trace.WriteLine($"Signal: '{signal.Name}' ({@event.MessageType})");
                        });
                }
            });

        #endregion

        return endpointConfiguration;
    })
    .Build();

await host.StartAsync();

var endpointInstance = host.Services.GetRequiredService<IMessageSession>();

try
{
    Console.WriteLine("Endpoint started. Press 'enter' to send a message");
    Console.WriteLine("Press any other key to quit");

    while (true)
    {
        var key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.Enter)
        {
            break;
        }

        await endpointInstance.SendLocal(new SomeCommand());
    }
}
finally
{
    await host.StopAsync();
}