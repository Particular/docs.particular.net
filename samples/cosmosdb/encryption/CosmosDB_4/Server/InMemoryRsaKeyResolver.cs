using System.Collections.Concurrent;
using System.Security.Cryptography;
using Azure.Core.Cryptography;

// Demo only: stores an unprotected RSA private key on disk. Use a secure key store in production.
class InMemoryRsaKeyResolver : IKeyEncryptionKeyResolver
{
    public const string ResolverName = "demo-inmemory-rsa";

    readonly ConcurrentDictionary<string, InMemoryRsaKey> keys = new();
    readonly string keyFilePath;

    public InMemoryRsaKeyResolver(string keyFilePath)
    {
        this.keyFilePath = keyFilePath;
    }

    public IKeyEncryptionKey Resolve(string keyId, CancellationToken cancellationToken = default) =>
        keys.GetOrAdd(keyId, id => new InMemoryRsaKey(id, keyFilePath));

    public Task<IKeyEncryptionKey> ResolveAsync(string keyId, CancellationToken cancellationToken = default) =>
        Task.FromResult(Resolve(keyId, cancellationToken));

    sealed class InMemoryRsaKey : IKeyEncryptionKey
    {
        readonly RSA rsa = RSA.Create(2048);

        public InMemoryRsaKey(string keyId, string keyFilePath)
        {
            KeyId = keyId;

            if (File.Exists(keyFilePath))
            {
                rsa.ImportRSAPrivateKey(File.ReadAllBytes(keyFilePath), out _);
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(keyFilePath)!);
                File.WriteAllBytes(keyFilePath, rsa.ExportRSAPrivateKey());
            }
        }

        public string KeyId { get; }

        public byte[] WrapKey(string algorithm, ReadOnlyMemory<byte> key, CancellationToken cancellationToken = default) =>
            rsa.Encrypt(key.ToArray(), Padding(algorithm));

        public Task<byte[]> WrapKeyAsync(string algorithm, ReadOnlyMemory<byte> key, CancellationToken cancellationToken = default) =>
            Task.FromResult(WrapKey(algorithm, key, cancellationToken));

        public byte[] UnwrapKey(string algorithm, ReadOnlyMemory<byte> encryptedKey, CancellationToken cancellationToken = default) =>
            rsa.Decrypt(encryptedKey.ToArray(), Padding(algorithm));

        public Task<byte[]> UnwrapKeyAsync(string algorithm, ReadOnlyMemory<byte> encryptedKey, CancellationToken cancellationToken = default) =>
            Task.FromResult(UnwrapKey(algorithm, encryptedKey, cancellationToken));

        static RSAEncryptionPadding Padding(string algorithm) => algorithm switch
        {
            "RSA-OAEP" or "RsaOaep" => RSAEncryptionPadding.OaepSHA1,
            "RSA-OAEP-256" => RSAEncryptionPadding.OaepSHA256,
            _ => throw new NotSupportedException($"Unsupported key wrap algorithm '{algorithm}'.")
        };
    }
}
