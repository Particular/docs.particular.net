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
        logger.LogInformation("Starting application...");
        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             Console.Title = "Server";
             services.AddLogging();

         }).UseNServiceBus(x =>
         {
             logger.LogInformation("Configuring NServiceBus...");

             #region CosmosDBConfig
             var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Container.Server");

             var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
             var connection = @"AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
             persistence.DatabaseName("Samples.CosmosDB.Container");
             var cosmosClient = new CosmosClient(connection);
             persistence.CosmosClient(cosmosClient);
             persistence.DefaultContainer("OrderSagaData", "/id");

             #endregion

             endpointConfiguration.UseTransport(new LearningTransport());
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.EnableInstallers();

             #region ContainerInformationFromLogicalMessage
             var transactionInformation = persistence.TransactionInformation();
             transactionInformation.ExtractContainerInformationFromMessage<ShipOrder>(m =>
             {
                 logger.LogInformation("Message '{MessageType}' destined to be handled by '{SagaType}' will use 'ShipOrderSagaData' container.", m.GetType().AssemblyQualifiedName, nameof(ShipOrderSaga));
                 return new ContainerInformation("ShipOrderSagaData", new PartitionKeyPath("/id"));
             });
             #endregion
             #region ContainerInformationFromHeaders
             transactionInformation.ExtractContainerInformationFromHeaders(headers =>
             {
                 if (headers.TryGetValue(Headers.SagaType, out var sagaTypeHeader) && sagaTypeHeader.Contains(nameof(ShipOrderSaga)))
                 {
                     logger.LogInformation("Message '{EnclosedMessageTypes}' destined to be handled by '{SagaType}' will use 'ShipOrderSagaData' container.", headers[Headers.EnclosedMessageTypes], nameof(ShipOrderSaga));

                     return new ContainerInformation("ShipOrderSagaData", new PartitionKeyPath("/id"));
                 }
                 return null;
             });
             #endregion

             cosmosClient.CreateDatabaseIfNotExistsAsync("Samples.CosmosDB.Container");
             var database = cosmosClient.GetDatabase("Samples.CosmosDB.Container");
             database.CreateContainerIfNotExistsAsync("ShipOrderSagaData", "/id");

             Console.ReadKey();

             return endpointConfiguration;
         });
}