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

        #region Create CosmosDb resources

        await Helper.CreateContainerAndDbIfNotExists(connectionString);
        #endregion

        #region ConfigurePersistence

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();

        endpointConfiguration.UsePersistence<CosmosPersistence>()
                    .CosmosClient(new CosmosClient(connectionString))
                    .DatabaseName("Samples.Database.Demo");

        persistence.DefaultContainer("Outbox", "/messageId");
        #endregion

        endpointConfiguration.EnableInstallers();

        #region SampleSteps

        // STEP 1: Run code as is, duplicates can be observed in console and database
        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractPartitionKeyFromHeader("PartitionKeyHeader");

        // STEP 2: Uncomment this line to enable the Outbox. Duplicates will be suppressed.
       // endpointConfiguration.EnableOutbox();

        // STEP 3: Comment out this line to allow concurrent processing. Concurrency exceptions will
        // occur in the console window, but only 5 entries will be made in the database.
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

        #endregion
        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Endpoint started. Press Enter to send 5 sets of duplicate messages...");
        Console.ReadLine();

        for (var i = 0; i < 5; i++)
        {
            var myMessage = new MyMessage();
            await Helper.SendDuplicates(endpointInstance, myMessage, totalCount: 2);
        }

        await Task.Delay(5000);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
