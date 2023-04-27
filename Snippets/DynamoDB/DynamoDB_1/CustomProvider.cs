using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

#region DynamoDBCustomClientProvider

class CustomDynamoClientProvider
    : IProvideDynamoClient
{
    // get fully configured via DI
    public CustomDynamoClientProvider(AmazonDynamoDBClient dynamoClient)
    {
        Client = dynamoClient;
    }
    public AmazonDynamoDBClient Client { get; }
}
#endregion

class DynamoDBCustomClientProviderRegistration
{
    public DynamoDBCustomClientProviderRegistration(EndpointConfiguration endpointConfiguration)
    {
        #region DynamoDBCustomClientProviderRegistration

        endpointConfiguration.RegisterComponents(c => c.AddTransient<CustomDynamoClientProvider>());

        #endregion
    }
}