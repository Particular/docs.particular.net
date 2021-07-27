using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CosmosDB.Container.Server";

        #region CosmosDBConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Container.Server");

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
        var connection = @"AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        persistence.DatabaseName("Samples.CosmosDB.Container");
        var cosmosClient = new CosmosClient(connection);
        persistence.CosmosClient(cosmosClient);
        persistence.DefaultContainer("OrderSagaData", "/id");

        #endregion

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableInstallers();

        #region BehaviorRegistration

        endpointConfiguration.Pipeline.Register(new BehaviorProvidingDynamicContainer(), "Provides a non-default container for sagas started by ship order message");

        #endregion

        await cosmosClient.CreateDatabaseIfNotExistsAsync("Samples.CosmosDB.Container");
        var database = cosmosClient.GetDatabase("Samples.CosmosDB.Container");
        await database.CreateContainerIfNotExistsAsync("ShipOrderSagaData", "/id");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}