using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Persistence.AzureTable;

 #region CustomClientProvider

class CustomTableClientProvider : IProvideTableServiceClient
{
    // get fully configured via DI container
    public CustomTableClientProvider(TableServiceClient tableServiceClient)
    {
        Client = tableServiceClient;
    }
    public TableServiceClient Client { get; }
}

// optionally when subscriptions used
class CustomSubscriptionTableClientProvider : IProvideTableServiceClientForSubscriptions
{
    // get fully configured via DI container
    public CustomSubscriptionTableClientProvider(TableServiceClient tableServiceClient)
    {
        Client = tableServiceClient;
    }
    public TableServiceClient Client { get; }
}
#endregion

class CustomClientProviderRegistration
{
    public CustomClientProviderRegistration(IHostApplicationBuilder builder)
    {
        #region CustomClientProviderRegistration

        builder.Services.AddSingleton<CustomTableClientProvider>();

        // optionally when subscriptions used
        builder.Services.AddSingleton<CustomSubscriptionTableClientProvider>();

        #endregion
    }
}
