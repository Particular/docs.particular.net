using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Frontend";
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.UseConsoleLifetime();

        builder.UseNServiceBus(builderContext =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.TransactionalSession.Frontend");
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport(new LearningTransport() { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });
            endpointConfiguration.EnableOutbox();

            endpointConfiguration.EnableTransactionalSession();

            var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
            persistence.CosmosClient(new CosmosClient(
                "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="));
            persistence.TransactionInformation().ExtractPartitionKeyFromHeaders(headers =>
            {
                if (headers.TryGetValue("CosmosPartitionKey", out var partitionKey))
                {
                    return new PartitionKey(partitionKey);
                }

                return PartitionKey.Null;
            });
            persistence.DefaultContainer("Orders", "/CustomerId");

            endpointConfiguration.PurgeOnStartup(true);

            return endpointConfiguration;
        });


        return builder.ConfigureServices(services => { services.AddHostedService<Worker>(); });
    }
}
