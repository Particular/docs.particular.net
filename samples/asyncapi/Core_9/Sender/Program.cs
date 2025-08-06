using Microsoft.Extensions.Hosting;
using NServiceBus;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        //services.AddHostedService<Worker>();
    })
    .UseNServiceBus(builder =>
    {
        var endpointConfiguration = new EndpointConfiguration("Sender");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.EnableInstallers();
        return endpointConfiguration;
    })
    .Build()
    .RunAsync();