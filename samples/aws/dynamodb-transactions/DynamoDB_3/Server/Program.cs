using System;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Server";

var builder = Host.CreateApplicationBuilder(args);

#region DynamoDBConfig

var amazonDynamoDbClient = new AmazonDynamoDBClient(
    new BasicAWSCredentials("localdb", "localdb"),
    new AmazonDynamoDBConfig
    {
        ServiceURL = "http://localhost:8000"
    });


var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Transactions.Server");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var persistence = endpointConfiguration.UsePersistence<DynamoPersistence>();
persistence.DynamoClient(amazonDynamoDbClient);
persistence.UseSharedTable(new TableConfiguration
{
    TableName = "Samples.DynamoDB.Transactions"
});

endpointConfiguration.EnableOutbox();

#endregion

endpointConfiguration.UseTransport(new LearningTransport
{
    TransportTransactionMode = TransportTransactionMode.ReceiveOnly
});
endpointConfiguration.EnableInstallers();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
