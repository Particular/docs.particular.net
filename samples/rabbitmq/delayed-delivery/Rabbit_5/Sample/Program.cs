using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static readonly ILog log = LogManager.GetLogger<Program>();

    static async Task Main()
    {
        Console.Title = "Samples.RabbitMQ.DelayedDelivery";

        var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.DelayedDelivery");

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseConventionalRoutingTopology();
        transport.ConnectionString("host=localhost");

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        var sendOptions = new SendOptions();
        sendOptions.RouteToThisEndpoint();
        sendOptions.DelayDeliveryWith(TimeSpan.FromSeconds(7));

        log.Info("Sending message to be delivered in 7 seconds...");
        await endpointInstance.Send(new MyMessage(), sendOptions);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop();
    }
}