using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CosmosDB.Simple.Server";

        #region CosmosDBConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Simple.Server");

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
        var connection = @"AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        persistence.DatabaseName("Samples.CosmosDB.Simple");
        persistence.CosmosClient(new CosmosClient(connection));
        persistence.DefaultContainer("Server", "/id");

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