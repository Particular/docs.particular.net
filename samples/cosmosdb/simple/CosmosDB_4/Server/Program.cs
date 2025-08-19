using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

             var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Simple.Server");

             var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
             var connection = Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING")
                 ?? """AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==""";
             persistence.DatabaseName("Samples.CosmosDB.Simple");
             persistence.CosmosClient(new(connection));
             persistence.DefaultContainer("Server", "/id");

             #endregion

             endpointConfiguration.UseTransport(new LearningTransport());
             endpointConfiguration.UseSerialization<SystemJsonSerializer>();
             endpointConfiguration.EnableInstallers();

             return endpointConfiguration;
         });

}