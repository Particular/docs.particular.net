using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.Kafka;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Kafka.SimpleReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.Kafka.SimpleReceiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        var transport = endpointConfiguration.UseTransport<KafkaTransport>();
        transport.ConnectionString("127.0.0.1:9092");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.WriteLine("Waiting for message from the Sender");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

}