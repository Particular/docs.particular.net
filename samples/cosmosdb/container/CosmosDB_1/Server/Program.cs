using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus;
using NServiceBus.Logging;
using Headers = NServiceBus.Headers;

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

        #region ContainerInformationFromLogicalMessage
        var transactionInformation = persistence.TransactionInformation();
        transactionInformation.ExtractContainerInformationFromMessage<ShipOrder>(m =>
        {
            Log.Info($"Message '{m.GetType().AssemblyQualifiedName}' destined to be handled by '{nameof(ShipOrderSaga)}' will use 'ShipOrderSagaData' container.");
            return new ContainerInformation("ShipOrderSagaData", new PartitionKeyPath("/id"));
        });
        #endregion
        #region ContainerInformationFromHeaders
        transactionInformation.ExtractContainerInformationFromHeaders(headers =>
        {
            if (headers.TryGetValue(Headers.SagaType, out var sagaTypeHeader) && sagaTypeHeader.Contains(nameof(ShipOrderSaga)))
            {
                Log.Info($"Message '{headers[Headers.EnclosedMessageTypes]}' destined to be handled by '{nameof(ShipOrderSaga)}' will use 'ShipOrderSagaData' container.");

                return new ContainerInformation("ShipOrderSagaData", new PartitionKeyPath("/id"));
            }
            return null;
        });
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

    static readonly ILog Log = LogManager.GetLogger<Program>();
}