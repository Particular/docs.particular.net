using Amazon.DynamoDBv2;
using NServiceBus;

namespace DynamoDB_1;

public class Usage
{
    void Configuration(EndpointConfiguration endpointConfiguration)
    {
        #region DynamoDBUsage
        var persistence = endpointConfiguration.UsePersistence<DynamoPersistence>();
        // optional client
        persistence.DynamoClient(new AmazonDynamoDBClient());
        #endregion
    }
}