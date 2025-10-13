using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Headers = NServiceBus.Headers;

class Program
{
    private static readonly ILogger<Program> logger =
      LoggerFactory.Create(builder =>
      {
          builder.AddConsole();
      }).CreateLogger<Program>();

    public static async Task Main(string[] args)
    {
        Console.Title = "Server";
        logger.LogInformation("Starting application...");
        
        var builder = Host.CreateApplicationBuilder(args);

        logger.LogInformation("Configuring NServiceBus...");

        #region CosmosDBConfig
        var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Container.Server");

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
        
        // Get connection string from environment variable, fallback to local emulator
        var connection = Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING") 
            ?? @"AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        
        logger.LogInformation($"Using Cosmos DB connection: {(Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING") != null ? "Azure service" : "Local emulator")}");
        
        persistence.DatabaseName("Samples.CosmosDB.Container");
        var cosmosClient = new CosmosClient(connection);
        persistence.CosmosClient(cosmosClient);
        persistence.DefaultContainer("OrderSagaData", "/id");

        #endregion

        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.EnableInstallers();
#pragma warning disable NSBC001 // EnableContainerFromMessageExtractor should be called when using both default container and message extractors
        #region ContainerInformationFromLogicalMessage
        var transactionInformation = persistence.TransactionInformation();

        transactionInformation.ExtractContainerInformationFromMessage<ShipOrder>(m =>
        {
            logger.LogInformation($"Message '{m.GetType().AssemblyQualifiedName}' destined to be handled by '{nameof(ShipOrderSaga)}' will use 'ShipOrderSagaData' container.");
            return new ContainerInformation("ShipOrderSagaData", new PartitionKeyPath("/id"));
        });
        #endregion
#pragma warning restore NSBC001 // EnableContainerFromMessageExtractor should be called when using both default container and message extractors
        #region ContainerInformationFromHeaders
        transactionInformation.ExtractContainerInformationFromHeaders(headers =>
        {
            if (headers.TryGetValue(Headers.SagaType, out var sagaTypeHeader) && sagaTypeHeader.Contains(nameof(ShipOrderSaga)))
            {
                logger.LogInformation($"Message '{headers[Headers.EnclosedMessageTypes]}' destined to be handled by '{nameof(ShipOrderSaga)}' will use 'ShipOrderSagaData' container.");

                return new ContainerInformation("ShipOrderSagaData", new PartitionKeyPath("/id"));
            }
            return null;
        });
        #endregion

        cosmosClient.CreateDatabaseIfNotExistsAsync("Samples.CosmosDB.Container").Wait();
        var database = cosmosClient.GetDatabase("Samples.CosmosDB.Container");
        database.CreateContainerIfNotExistsAsync("ShipOrderSagaData", "/id").Wait();

        builder.UseNServiceBus(endpointConfiguration);

        var host = builder.Build();
        await host.StartAsync();

        logger.LogInformation("Server started successfully. Press any key to exit");

        // Wait for input or termination
        if (Console.IsInputRedirected)
        {
            // In container or background mode, wait for termination signal
            await host.WaitForShutdownAsync();
        }
        else
        {
            // Interactive mode
            Console.ReadKey();
            await host.StopAsync();
        }
    }
}