using NServiceBus;
using Shared;
//using System.Threading;

var config = new EndpointConfiguration("KubernetesDemo.Publisher");
config.UseSerialization<SystemJsonSerializer>();

config.EnableInstallers();

var transport = new LearningTransport
{
    StorageDirectory = "transport"
};
config.UseTransport(transport);

config.Recoverability().Immediate(r => r.NumberOfRetries(0)).Delayed(d => d.NumberOfRetries(0));

var endpoint = await Endpoint.Start(config);
Console.WriteLine("Publishing endpoint started");

var messageId = Guid.NewGuid().ToString();

Console.WriteLine($"Publishing event {messageId}");
await endpoint.Publish(new DemoEvent() { Id = messageId });


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