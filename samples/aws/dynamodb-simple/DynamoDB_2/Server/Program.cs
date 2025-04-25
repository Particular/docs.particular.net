using System;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

Console.Title = "Server";

var builder = Host.CreateApplicationBuilder(args);
#region DynamoDBConfig

var amazonDynamoDbClient = new AmazonDynamoDBClient(
    new BasicAWSCredentials("localdb", "localdb"),
    new AmazonDynamoDBConfig
    {
        ServiceURL = "http://localhost:8000"
    });

var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Simple.Server");

var persistence = endpointConfiguration.UsePersistence<DynamoPersistence>();
persistence.DynamoClient(amazonDynamoDbClient);
persistence.UseSharedTable(new TableConfiguration
{
    TableName = "Samples.DynamoDB.Simple"
});

#endregion

endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.EnableInstallers();


Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
