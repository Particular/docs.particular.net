using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Monitor3rdParty";

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, _) =>
    {
        // Add any services here if needed
    })
    .UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomChecks.Monitor3rdParty");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.ReportCustomChecksTo("Particular.ServiceControl");

        return endpointConfiguration;
    })
    .Build()
    .RunAsync();