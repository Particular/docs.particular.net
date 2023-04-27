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

        var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Simple.Server");

        var persistence = endpointConfiguration.UsePersistence<DynamoPersistence>();
        var credentials = new BasicAWSCredentials("test","test");
        persistence.DynamoClient(new AmazonDynamoDBClient(credentials, new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:4566",
            AuthenticationRegion = "eu-central-1"
        }));
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