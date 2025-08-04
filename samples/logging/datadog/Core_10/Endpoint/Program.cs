using System;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Microsoft.Extensions.Hosting;

const string EndpointName = "Samples.Metrics.Tracing.Endpoint";

Console.Title = EndpointName;

var host = Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .UseNServiceBus(_ =>
    {
        var endpointConfiguration = new EndpointConfiguration(EndpointName);

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        DataDogMetrics.Setup(endpointConfiguration);

        return endpointConfiguration;
    })
    .Build();

await host.StartAsync();

var endpointInstance = host.Services.GetRequiredService<IMessageSession>();

var simulator = new LoadSimulator(endpointInstance, TimeSpan.Zero, TimeSpan.FromSeconds(10));
await simulator.Start();

try
{
    Console.WriteLine("Endpoint started.");
    Console.WriteLine("Press [ENTER] to send additional messages.");
    Console.WriteLine("Press [Q] to quit.");

    while (true)
    {
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Q)
        {
            break;
        }

        await endpointInstance.SendLocal(new SomeCommand());
    }
}
finally
{
    await simulator.Stop();
    await host.StopAsync();
}