using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NServiceBus;
using Sender;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((ctx, logging) =>
    {
        logging.ClearProviders();
        logging.AddConsole(options => { options.FormatterName = ConsoleFormatterNames.Simple; });
    })
    .UseNServiceBus(hostContext =>
    {
        var endpointConfiguration = new EndpointConfiguration("Sender");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.SendOnly();

        var azureServiceBus = new AzureServiceBusTransport(hostContext.Configuration["FullyQualifiedNamespace"], new DefaultAzureCredential());
        endpointConfiguration.UseTransport(azureServiceBus);

        return endpointConfiguration;
    })
    .ConfigureServices((builder, services) =>
    {
        services.Configure<Settings>(builder.Configuration.GetSection(key: nameof(Settings)));

        services.AddHostedService<SendMessages>();
    })
    .Build();
await host.RunAsync();