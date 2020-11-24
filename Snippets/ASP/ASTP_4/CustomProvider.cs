using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.AzureTable;

 #region CustomClientProvider

class CustomTableClientProvider
    : IProvideCloudTableClient
{
    // get fully configured via DI container
    public CustomTableClientProvider(CloudTableClient tableClient)
    {
        Client = tableClient;
    }
    public CloudTableClient Client { get; }
}

// optionally when subscriptions used
class CustomSubscriptionTableClientProvider
    : IProvideCloudTableClientForSubscriptions
{
    // get fully configured via DI container
    public CustomSubscriptionTableClientProvider(CloudTableClient tableClient)
    {
        Client = tableClient;
    }
    public CloudTableClient Client { get; }
}
#endregion

class CustomClientProviderRegistration
{
    public CustomClientProviderRegistration(EndpointConfiguration endpointConfiguration)
    {
        #region CustomClientProviderRegistration

        endpointConfiguration.RegisterComponents(services => services.AddSingleton<CustomTableClientProvider>());

        // optionally when subscriptions used
        endpointConfiguration.RegisterComponents(services => services.AddSingleton<CustomSubscriptionTableClientProvider>());

        #endregion
    }
}
