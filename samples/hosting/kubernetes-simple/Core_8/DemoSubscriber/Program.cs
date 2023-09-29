using NServiceBus;

var config = new EndpointConfiguration("KubernetesDemo.Subscriber");
config.UseSerialization<SystemJsonSerializer>();

config.EnableInstallers();

var transport = new LearningTransport
{
    StorageDirectory = "/transport"
};
config.UseTransport(transport);

var persistence = config.UsePersistence<LearningPersistence>();
persistence.SagaStorageDirectory("/sagas");

config.Recoverability().Immediate(r => r.NumberOfRetries(0)).Delayed(d => d.NumberOfRetries(0));

var endpoint = await Endpoint.Start(config);
Console.WriteLine("Endpoint started");

while (true)
{
    if (Console.IsInputRedirected)
    {
        await Task.Delay(10000);
        continue;
    }

    Console.WriteLine("Press [Esc] to exit");
    var key = Console.ReadKey();
    if (key.KeyChar == (int)ConsoleKey.Escape)
    {
        break;
    }


    Console.WriteLine();
}

await endpoint.Stop();