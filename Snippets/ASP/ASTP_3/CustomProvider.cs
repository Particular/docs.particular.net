using Microsoft.Azure.Cosmos.Table;
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

        endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<CustomTableClientProvider>(DependencyLifecycle.SingleInstance));

        // optionally when subscriptions used
        endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<CustomSubscriptionTableClientProvider>(DependencyLifecycle.SingleInstance));

        #endregion
    }
}
