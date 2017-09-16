using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.Kafka;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Kafka.SimpleSender";
        var endpointConfiguration = new EndpointConfiguration("Samples.Kafka.SimpleSender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();

        #region TransportConfiguration

        var transport = endpointConfiguration.UseTransport<KafkaTransport>();
        transport.ConnectionString("127.0.0.1:9092");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await endpointInstance.Send("Samples.Kafka.SimpleReceiver", new MyMessage())
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}