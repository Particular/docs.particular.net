using Infrastructure;
using Microsoft.Extensions.Hosting;
using NServiceBus;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        //services.AddHostedService<Worker>();
    })
    .UseNServiceBus(builder =>
    {
        var endpointConfiguration = new EndpointConfiguration("Subscriber");
        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.EnableAsyncApiSupport();

        endpointConfiguration.EnableInstallers();
        return endpointConfiguration;
    })
    .Build()
    .RunAsync();