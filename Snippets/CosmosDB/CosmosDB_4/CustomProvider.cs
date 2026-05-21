using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Persistence.CosmosDB;

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
    public CosmosDBCustomClientProviderRegistration(IHostApplicationBuilder builder)
    {
        #region CosmosDBCustomClientProviderRegistration

        builder.Services.AddTransient<IProvideCosmosClient, CustomCosmosClientProvider>();

        #endregion
    }
}