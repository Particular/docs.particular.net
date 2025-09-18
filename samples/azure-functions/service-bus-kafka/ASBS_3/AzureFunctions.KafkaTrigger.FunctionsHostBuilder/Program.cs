using AzureFunctions.Messages.NServiceBusMessages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region SetupNServiceBusSendOnly

var host = new HostBuilder()
    .ConfigureServices(async services =>
    {
        var cfg = new EndpointConfiguration("SendOnly");
        cfg.SendOnly();
        cfg.UseSerialization<SystemJsonSerializer>();

        var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsServiceBus");
        var transport = new AzureServiceBusTransport(connectionString);
        var routing = cfg.UseTransport(transport);

        routing.RouteToEndpoint(typeof(FollowUp), "Samples.KafkaTrigger.ConsoleEndpoint");

        var endpoint = await Endpoint.Start(cfg);

        // Inject the endpoint in the DI container
        services.AddSingleton<IMessageSession>(endpoint);
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

#endregion

await host.RunAsync();