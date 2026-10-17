using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "RabbitMQCosmosDBOutbox";

        #region ConfigureTransport
        var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDb.Outbox");

        var rabbitMqTransport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic), "host=localhost;username=rabbitmq;password=rabbitmq")
        {
            TransportTransactionMode = TransportTransactionMode.ReceiveOnly
        };
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(rabbitMqTransport);
        #endregion

        //add your own account here
        var connectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        #region ConfigurePersistence

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();

        persistence.CosmosClient(new CosmosClient(connectionString, new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway
        }));
        persistence.DatabaseName("Samples.Database.Demo");

        // Business data and Outbox records share this container and are partitioned by order
        persistence.DefaultContainer("Orders", "/OrderId");

        // Creates the database and container if they don't exist
        endpointConfiguration.EnableInstallers();
        #endregion

        #region PartitionKeyFromMessage

        // The partition key is read from the deserialized message, so the Outbox runs in the logical pipeline stage
        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromMessage<ProcessOrder>(message => new PartitionKey(message.OrderId.ToString()));
        #endregion

        #region SampleSteps

        // STEP 1: Run code as is. Duplicates are processed twice and stored twice.

        // STEP 2: Uncomment this line to enable the Outbox. Duplicates will be skipped.
        // endpointConfiguration.EnableOutbox();

        // STEP 3: Comment out this line to allow concurrent processing. Duplicates may be processed
        // concurrently, but only one attempt per message commits its business data and Outbox record.
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

        #endregion
        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Endpoint started. Press Enter to send 5 sets of duplicate messages...");
        Console.ReadLine();

        for (var i = 0; i < 5; i++)
        {
            var processOrder = new ProcessOrder { OrderId = Guid.NewGuid() };
            await Helper.SendDuplicates(endpointInstance, processOrder, totalCount: 2);
        }

        await Task.Delay(5000);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
