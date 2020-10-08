using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CosmosDB.Transactions.Server";

        #region CosmosDBConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Transactions.Server");
        endpointConfiguration.EnableOutbox();

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
        var connection = @"AccountEndpoint = https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        persistence.DatabaseName("Samples.CosmosDB.Transactions");
        persistence.CosmosClient(new CosmosClient(connection));
        persistence.DefaultContainer("Server", "/OrderId");

        #endregion

        #region BehaviorRegistration

        endpointConfiguration.Pipeline.Register(new OrderIdHeaderAsPartitionKeyBehavior(), "Extracts a partition key from a header");
        endpointConfiguration.Pipeline.Register(new OrderIdAsPartitionKeyBehavior.Registration());

        #endregion

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}