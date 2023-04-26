using Amazon.DynamoDBv2;
using NServiceBus;

namespace DynamoDB_1;

public class Usage
{
    void Configuration(EndpointConfiguration endpointConfiguration)
    {
        #region DynamoDBUsage
        var persistence = endpointConfiguration.UsePersistence<DynamoDBPersistence>();
        // optional client
        persistence.DynamoDBClient(new AmazonDynamoDBClient());
        #endregion
    }
}