using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.DynamoDB;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.DynamoDB.Transactions.Server";

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

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}