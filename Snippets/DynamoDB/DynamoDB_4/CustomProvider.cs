namespace DynamoDB_4;

using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

#region DynamoDBCustomClientProvider

class CustomDynamoClientProvider
    : IDynamoClientProvider
{
    // get fully configured via DI
    public CustomDynamoClientProvider(IAmazonDynamoDB dynamoClient)
    {
        Client = dynamoClient;
    }
    public IAmazonDynamoDB Client { get; }
}
#endregion

class DynamoDBCustomClientProviderRegistration
{
    public DynamoDBCustomClientProviderRegistration(EndpointConfiguration endpointConfiguration)
    {
        #region DynamoDBCustomClientProviderRegistration

        endpointConfiguration.RegisterComponents(c => c.AddTransient<IDynamoClientProvider, CustomDynamoClientProvider>());

        #endregion
    }
}