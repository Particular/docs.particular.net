using Infrastructure;
using Microsoft.Extensions.Hosting;

Console.Title = "AsyncAPI Subscriber";

await Host.CreateDefaultBuilder(args)
    .UseNServiceBus(builder =>
    {
        var endpointConfiguration = new EndpointConfiguration("Subscriber");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.EnableAsyncApiSupport();

        endpointConfiguration.EnableInstallers();
        return endpointConfiguration;
    })
    .Build()
    .RunAsync();