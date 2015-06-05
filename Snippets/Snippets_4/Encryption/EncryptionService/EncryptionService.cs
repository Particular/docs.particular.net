namespace Snippets4.Encryption.EncryptionService
{
    using NServiceBus;
    using NServiceBus.Encryption;

    public class EncryptionService : IEncryptionService
    {
        public EncryptedValue Encrypt(string value)
        {
            return null;
        }

        public string Decrypt(EncryptedValue encryptedValue)
        {
            return null;
        }
    }
}