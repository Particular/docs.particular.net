using Confluent.Kafka;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        const string endpointName = "Samples.KafkaTrigger.ConsoleEndpoint";
        Console.Title = endpointName;

        #region NServiceBus

        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.EnableInstallers();

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var transport = new AzureServiceBusTransport(connectionString);
        endpointConfiguration.UseTransport(transport);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #endregion

        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = "producer-1"
        };

        Console.WriteLine("Press 'enter' to send an event using Kafka and wait for a response...");
        Console.WriteLine("Press any other key to exit");

        using (var producer = new ProducerBuilder<string, string>(config).Build())
        {

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }

                var message = new Message<string, string>
                {
                    Value = $"It is now {DateTime.UtcNow}"
                };
                var deliveryResult = await producer.ProduceAsync("input-topic", message);


                Console.WriteLine("Message1 sent");
            }
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}