using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "TracingEndpoint";

var builder = Host.CreateApplicationBuilder(args);

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

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

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

        await messageSession.SendLocal(new SomeCommand());
    }
}
finally
{
    await host.StopAsync();
}