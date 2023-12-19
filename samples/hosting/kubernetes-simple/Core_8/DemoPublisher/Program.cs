using NServiceBus;

Console.Title = "Demo Publisher";

var builder = Host.CreateDefaultBuilder(args);
builder.UseConsoleLifetime();
builder.ConfigureLogging(logging => logging.AddConsole());
builder.UseNServiceBus(ctx =>
{
    var endpointConfiguration = new EndpointConfiguration("KubernetesDemo.Publisher");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

    endpointConfiguration.EnableInstallers();

    var transport = new LearningTransport
    {
        StorageDirectory = "transport"
    };
    endpointConfiguration.UseTransport(transport);

    endpointConfiguration.Recoverability().Immediate(r => r.NumberOfRetries(0)).Delayed(d => d.NumberOfRetries(0));

    return endpointConfiguration;
});
builder.ConfigureServices(services =>
{
    services.AddHostedService<PublisherService>();
});

var host = builder.Build();
await host.RunAsync();