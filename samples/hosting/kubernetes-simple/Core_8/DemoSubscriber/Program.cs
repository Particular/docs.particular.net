using NServiceBus;

Console.Title = "Demo Subscriber";

var builder = Host.CreateDefaultBuilder(args);
builder.UseConsoleLifetime();
builder.ConfigureLogging(logging => logging.AddConsole());
builder.UseNServiceBus(ctx =>
{
    var endpointConfiguration = new EndpointConfiguration("KubernetesDemo.Subscriber");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

    endpointConfiguration.EnableInstallers();

    var transport = new LearningTransport
    {
        StorageDirectory = "transport"
    };
    endpointConfiguration.UseTransport(transport);

    var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
    persistence.SagaStorageDirectory("sagas");

    endpointConfiguration.Recoverability().Immediate(r => r.NumberOfRetries(0)).Delayed(d => d.NumberOfRetries(0));

    return endpointConfiguration;
});

var host = builder.Build();
await host.RunAsync();