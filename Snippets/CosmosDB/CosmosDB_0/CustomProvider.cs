using Microsoft.Azure.Cosmos;
using NServiceBus;

#region CosmosDBCustomClientProvider

class CustomCosmosClientProvider
    : IProvideCosmosClient
{
    // get fully configured via DI
    public CustomCosmosClientProvider(CosmosClient cosmosClient)
    {
        Client = cosmosClient;
    }
    public CosmosClient Client { get; }
}
#endregion

class CosmosDBCustomClientProviderRegistration
{
    public CosmosDBCustomClientProviderRegistration(EndpointConfiguration endpointConfiguration)
    {
        #region CosmosDBCustomClientProviderRegistration

        endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<CustomCosmosClientProvider>(DependencyLifecycle.SingleInstance));

        #endregion
    }
}