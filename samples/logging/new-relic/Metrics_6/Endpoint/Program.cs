using System;
using Microsoft.Extensions.Hosting;
using Endpoint;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

Console.Title = "TracingEndpoint";

var host = Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .UseNServiceBus(_ =>
    {
        var endpointConfiguration = new EndpointConfiguration("TracingEndpoint");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        NewRelicMetrics.Setup(endpointConfiguration);

        return endpointConfiguration;
    })
    .Build();

await host.StartAsync();

var endpointInstance = host.Services.GetRequiredService<IMessageSession>();

#region newrelic-load-simulator

var simulator = new LoadSimulator(endpointInstance, TimeSpan.Zero, TimeSpan.FromSeconds(10));
simulator.Start();

#endregion

try
{
    Console.WriteLine("Endpoint started.");
    Console.WriteLine("Press [ENTER] to send additional messages.");
    Console.WriteLine("Press [Q] to quit.");

    while (true)
    {
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.Q:
                return;
            case ConsoleKey.Enter:
                await endpointInstance.SendLocal(new SomeCommand());
                break;
        }
    }
}
finally
{
    await simulator.Stop();
    await host.StopAsync();
}