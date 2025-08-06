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
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        //endpointConfiguration.EnableAsyncApiSupport();
        var conventions = endpointConfiguration.Conventions();
        conventions.Add(new PublishedEventsConvention());
        conventions.Add(new SubscribedEventsConvention());

        endpointConfiguration.EnableInstallers();
        return endpointConfiguration;
    })
    .Build()
    .RunAsync();