using System;
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

        var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Transactions.Server");
        endpointConfiguration.EnableOutbox();

        var persistence = endpointConfiguration.UsePersistence<DynamoDBPersistence>();
        var credentials = new BasicAWSCredentials("test","test");
        persistence.DynamoDBClient(new AmazonDynamoDBClient(credentials, new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:4566",
            AuthenticationRegion = "eu-central-1"
        }));
        persistence.UseSharedTable(new TableConfiguration
        {
            TableName = "Samples.DynamoDB.Transactions"
        });

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

    static ILog Log = LogManager.GetLogger<Program>();
}