using Azure.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NServiceBus;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((ctx, logging) =>
    {
        logging.ClearProviders();
        logging.AddConsole(options => { options.FormatterName = ConsoleFormatterNames.Simple; });
    })
    .UseNServiceBus(hostContext =>
    {
        var endpointConfiguration = new EndpointConfiguration("Receiver");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var azureServiceBus = new AzureServiceBusTransport(hostContext.Configuration["FullyQualifiedNamespace"], new DefaultAzureCredential());
        endpointConfiguration.UseTransport(azureServiceBus);

        return endpointConfiguration;
    })
    .Build();
await host.RunAsync();
