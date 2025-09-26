using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        Console.Title = "Endpoint";
        services.AddHostedService<InputLoopService>();
    })
    .UseNServiceBus(x =>
    {
        var endpointConfiguration = new EndpointConfiguration(
            "Samples.Bridge.Endpoint");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport
        {
            StorageDirectory = $"{LearningTransportInfrastructure.FindStoragePath()}2"
        });

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            customizations: immediate => { immediate.NumberOfRetries(0); });
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableMetrics()
            .SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        routing.RouteToEndpoint(typeof(MyMessage), "Samples.Bridge.Endpoint");

        return endpointConfiguration;
    })
    .Build();

await host.RunAsync();