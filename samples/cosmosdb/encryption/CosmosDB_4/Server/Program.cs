using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Encryption;
using Microsoft.Extensions.Hosting;

Console.Title = "Server";

var cosmosClient = await CreateEncryptingClientAsync();

var builder = Host.CreateApplicationBuilder(args);

#region CosmosDBConfig

var endpointConfiguration = new EndpointConfiguration("Samples.CosmosDB.Encryption.Server");

var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>();
persistence.DatabaseName("Samples.CosmosDB.Encryption");
persistence.CosmosClient(cosmosClient);
persistence.DefaultContainer("Server", "/id");
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
        cosmosClientOptions.ServerCertificateCustomValidationCallback = (_, _, _) => true;
    }

    var resolver = new InMemoryRsaKeyResolver(Path.Combine(AppContext.BaseDirectory, ".demo", "spike-kek.rsa"));
    var resolverName = InMemoryRsaKeyResolver.ResolverName;

    var cosmosClient = new CosmosClient(connection, cosmosClientOptions)
        .WithEncryption(resolver, resolverName);

    await cosmosClient.CreateDatabaseIfNotExistsAsync("Samples.CosmosDB.Encryption");

    await EncryptionSetup.CreateClientEncryptionKeyIfNotExistsAsync(cosmosClient, resolverName);

    await DropContainerAsync(cosmosClient, "Server");
    await EncryptionSetup.CreateEncryptedContainerIfNotExistsAsync(cosmosClient);

    return cosmosClient;
}

static async Task DropContainerAsync(CosmosClient client, string containerName)
{
    try
    {
        await client.GetContainer("Samples.CosmosDB.Encryption", containerName).DeleteContainerAsync();
    }
    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
    }
}
