using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             Console.Title = "Server";

         }).UseNServiceBus(x =>
         {
             #region CosmosDBConfig

             var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Transactions.Server");
             endpointConfiguration.EnableOutbox();

             var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
             var connection = Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING") ??
                             @"AccountEndpoint = https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
             persistence.DatabaseName("Samples.CosmosDB.Transactions");
             persistence.CosmosClient(new CosmosClient(connection));
             persistence.DefaultContainer("Server", "/OrderId");

             #endregion

             #region TransactionInformationFromLogicalMessage

             var transactionInformation = persistence.TransactionInformation();
             transactionInformation.ExtractPartitionKeyFromMessage<IProvideOrderId>(provideOrderId =>
             {
                 logger.LogInformation($"Found partition key '{provideOrderId.OrderId}' from '{nameof(IProvideOrderId)}'");
                 return new PartitionKey(provideOrderId.OrderId.ToString());
             });
             #endregion

             #region TransactionInformationFromHeader
             transactionInformation.ExtractPartitionKeyFromHeader("Sample.CosmosDB.Transaction.OrderId", orderId =>
             {
                 logger.LogInformation($"Found partition key '{orderId}' from header 'Sample.CosmosDB.Transaction'");
                 return orderId;
             });

             #endregion

             var transport = new LearningTransport
             {
                 TransportTransactionMode = TransportTransactionMode.ReceiveOnly
             };
             endpointConfiguration.UseTransport(transport);
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();

             endpointConfiguration.EnableInstallers();

             return endpointConfiguration;
         });


    private static readonly ILogger<Program> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<Program>();
}