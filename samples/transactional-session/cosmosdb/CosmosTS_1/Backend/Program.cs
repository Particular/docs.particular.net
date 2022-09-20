using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Backend";
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.UseConsoleLifetime();

        builder.UseNServiceBus(builderContext =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.TransactionalSession.Backend");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.Transactions(TransportTransactionMode.ReceiveOnly);
            endpointConfiguration.EnableOutbox();

            #region cosmos-txsession-backend-persistence
            var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
            persistence.CosmosClient(new CosmosClient(Configuration.CosmosDBConnectionString));
            persistence.DefaultContainer("Orders", "/CustomerId");
            persistence.TransactionInformation().ExtractPartitionKeyFromMessage<OrderReceived>(message => new PartitionKey(message.CustomerId));
            #endregion

            return endpointConfiguration;
        });


        return builder;
    }
}