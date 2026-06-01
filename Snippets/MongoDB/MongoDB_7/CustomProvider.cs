using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    public CustomMongoClientProviderRegistration(IHostApplicationBuilder builder)
    {
        #region MongoDBCustomClientProviderRegistration

        builder.Services.AddTransient<IMongoClientProvider, CustomMongoClientProvider>();

        #endregion
    }
}