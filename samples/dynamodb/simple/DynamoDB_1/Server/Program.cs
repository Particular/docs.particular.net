using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.DynamoDB.Simple.Server";

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
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}