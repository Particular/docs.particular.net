using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Encryption;
using Microsoft.Data.Encryption.Cryptography;

static class EncryptionSetup
{
    const string DatabaseName = "Samples.CosmosDB.Encryption";
    const string ContainerName = "Server";
    const string PartitionKeyPath = "/id";
    const string ClientEncryptionKeyId = "nsb-demo-key";

    public static async Task CreateClientEncryptionKeyIfNotExistsAsync(
        CosmosClient encryptingClient,
        string resolverName,
        CancellationToken cancellationToken = default)
    {
        var database = encryptingClient.GetDatabase(DatabaseName);

        await database.CreateClientEncryptionKeyAsync(
                ClientEncryptionKeyId,
                DataEncryptionAlgorithm.AeadAes256CbcHmacSha256,
                new EncryptionKeyWrapMetadata(
                    resolverName,
                    "demoKek",
                    "https://demo.local/keys/nsb-demo-kek/1",
                    "RSA-OAEP"),
                cancellationToken: cancellationToken);
    }

    public static async Task CreateEncryptedContainerIfNotExistsAsync(
        CosmosClient encryptingClient,
        CancellationToken cancellationToken = default)
    {
        var database = encryptingClient.GetDatabase(DatabaseName);

        #region EncryptionPolicy

        var containerProperties = new ContainerProperties(ContainerName, PartitionKeyPath)
        {
            DefaultTimeToLive = -1,

            ClientEncryptionPolicy = new ClientEncryptionPolicy(
            [
                // NServiceBus derives the saga ID from the correlation value and performs a point read,
                // so the correlated property can use randomized encryption.
                Encrypted("/OrderId"),
                Encrypted("/OrderDescription")

                // Deliberately NOT encrypted:
                //   id                                  - point-read key, and policy format 1 forbids it
                //   /PartitionKey                       - routing key, forbidden by policy format 1
                //   /_NServiceBus-Persistence-Metadata  - pessimistic locking patches a path INSIDE
                //                                         this object (SagaDataContainer-ReservedUntil).
                //                                         Only top-level paths can be encrypted, so
                //                                         including it would encrypt the whole subtree
                //                                         and break the lock.
            ])
        };

        #endregion

        await database.CreateContainerIfNotExistsAsync(containerProperties, cancellationToken: cancellationToken);
    }

    static ClientEncryptionIncludedPath Encrypted(string path) => new()
    {
        Path = path,
        ClientEncryptionKeyId = ClientEncryptionKeyId,
        EncryptionType = EncryptionType.Randomized.ToString(),
        EncryptionAlgorithm = DataEncryptionAlgorithm.AeadAes256CbcHmacSha256
    };
}
