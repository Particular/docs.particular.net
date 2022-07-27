using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using NServiceBus;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Backend";
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.UseConsoleLifetime();

        builder.UseNServiceBus(builderContext =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.TransactionalSession.Backend");
            endpointConfiguration.UseTransport(new LearningTransport { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });
            endpointConfiguration.EnableOutbox();

            var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
            persistence.CosmosClient(new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="));
            persistence.DefaultContainer("Orders", "/CustomerId");
            persistence.TransactionInformation().ExtractPartitionKeyFromMessage<OrderReceived>(message => new PartitionKey(message.CustomerId));

            return endpointConfiguration;
        });


        return builder;
    }
}