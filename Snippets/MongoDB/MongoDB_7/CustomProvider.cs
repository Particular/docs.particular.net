using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NServiceBus;
using NServiceBus.Storage.MongoDB;

#region MongoDBClientProvider

class CustomMongoClientProvider
    : IMongoClientProvider
{
    // get fully configured via DI
    public CustomMongoClientProvider(IMongoClient mongoClient)
    {
        Client = mongoClient;
    }
    public IMongoClient Client { get; }
}
#endregion

class CustomMongoClientProviderRegistration
{
    public CustomMongoClientProviderRegistration(EndpointConfiguration endpointConfiguration)
    {
        #region MongoDBCustomClientProviderRegistration

        endpointConfiguration.RegisterComponents(c => c.AddTransient<IMongoClientProvider, CustomMongoClientProvider>());

        #endregion
    }
}