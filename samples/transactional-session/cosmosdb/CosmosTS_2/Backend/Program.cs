using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Backend";

var builder = Host.CreateDefaultBuilder(args);
builder.UseConsoleLifetime();

builder.UseNServiceBus(builderContext =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.TransactionalSession.Backend");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.UseTransport(new LearningTransport { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });
    endpointConfiguration.EnableOutbox();

    #region cosmos-txsession-backend-persistence
    var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
    persistence.CosmosClient(new CosmosClient(Configuration.CosmosDBConnectionString));
    persistence.DefaultContainer("Orders", "/CustomerId");
    persistence.TransactionInformation().ExtractPartitionKeyFromMessage<OrderReceived>(message => new PartitionKey(message.CustomerId));
    #endregion

    return endpointConfiguration;
});

await builder.Build().RunAsync();