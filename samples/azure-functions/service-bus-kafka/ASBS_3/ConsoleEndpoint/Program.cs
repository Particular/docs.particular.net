using AzureFunctions.Messages.KafkaMessages;
using Confluent.Kafka;
using NServiceBus;

const string endpointName = "Samples.KafkaTrigger.ConsoleEndpoint";
Console.Title = endpointName;

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString);
endpointConfiguration.UseTransport(transport);

var endpointInstance = await Endpoint.Start(endpointConfiguration);

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9094",
    ClientId = "producer-1",
    BatchSize = 50
};

Console.WriteLine("Press '[enter]' to send a 100 events using Kafka and wait for a possible response...");
Console.WriteLine("Press any other key to exit");

using (var producer = new ProducerBuilder<string, string>(config)
           .Build())
{
    while (true)
    {
        var key = Console.ReadKey();
        Console.WriteLine();

        if (key.Key != ConsoleKey.Enter)
        {
            break;
        }

        for (int i = 0; i < 100; i++)
        {
            #region ProduceEvent

            var electricityUsage = new ElectricityUsage() { CustomerId = 42, CurrentUsage = i, UnitId = 1337 };

            var message = new Message<string, string>
            {
                Value = ElectricityUsage.Serialize(electricityUsage)
            };

            var deliveryResult = await producer.ProduceAsync("myKafkaTopic", message);

            #endregion
        }

        Console.WriteLine("100 messages sent");
    }
}

await endpointInstance.Stop();