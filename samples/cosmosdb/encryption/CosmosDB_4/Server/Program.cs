using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Encryption;
using Microsoft.Extensions.Hosting;

Console.Title = "Server";

using var cosmosClient = await CreateEncryptingClientAsync();

var builder = Host.CreateApplicationBuilder(args);

#region CosmosDBConfig

var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Encryption.Server");

var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
persistence.DatabaseName("Samples.CosmosDB.Encryption");
// NServiceBus must use the encryption-enabled client so saga reads and writes apply the encryption policy.
persistence.CosmosClient(cosmosClient);
persistence.DefaultContainer("Server", "/id");
// The container is created explicitly with its encryption policy, so persistence must not create it.
persistence.DisableContainerCreation();

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

#endregion

var host = builder.Build();

await host.RunAsync();

static async Task<CosmosClient> CreateEncryptingClientAsync()
{
    var connection = Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING")
        ?? """AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==""";

    var cosmosClientOptions = new CosmosClientOptions();
    if (connection.Contains("localhost", StringComparison.OrdinalIgnoreCase))
    {
        cosmosClientOptions.ConnectionMode = ConnectionMode.Gateway;
        cosmosClientOptions.LimitToEndpoint = true;
        // The emulator uses a development certificate. Never disable certificate validation for non-local endpoints.
        cosmosClientOptions.ServerCertificateCustomValidationCallback = (_, _, _) => true;
    }

    var resolver = new InMemoryRsaKeyResolver(Path.Combine(AppContext.BaseDirectory, ".demo", "spike-kek.rsa"));
    var resolverName = InMemoryRsaKeyResolver.ResolverName;

    var cosmosClient = new CosmosClient(connection, cosmosClientOptions)
        .WithEncryption(resolver, resolverName);

    // Demo only: recreating the database avoids stale encryption keys. Never delete a production database at startup.
    await DropDatabaseAsync(cosmosClient);
    await cosmosClient.CreateDatabaseIfNotExistsAsync("Samples.CosmosDB.Encryption");

    await EncryptionSetup.CreateClientEncryptionKeyIfNotExistsAsync(cosmosClient, resolverName);

    await EncryptionSetup.CreateEncryptedContainerIfNotExistsAsync(cosmosClient);

    return cosmosClient;
}

static async Task DropDatabaseAsync(CosmosClient client)
{
    try
    {
        await client.GetDatabase("Samples.CosmosDB.Encryption").DeleteAsync();
    }
    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
    }
}
