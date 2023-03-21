using Amazon.DynamoDBv2;
using Amazon.Runtime;
using NServiceBus;

namespace DynamoDB_1;

public class Usage
{
    void Configuration(EndpointConfiguration endpointConfiguration)
    {
        #region DynamoDBUsage
        var dynamoConfig = endpointConfiguration.UsePersistence<DynamoDBPersistence>();
        dynamoConfig.DynamoDBClient(new AmazonDynamoDBClient());
        #endregion
    }
}