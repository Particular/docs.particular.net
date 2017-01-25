namespace Core6.Encryption.EncryptionService
{
    using NServiceBus;
    using NServiceBus.Pipeline;

    public class EncryptionService :
        IEncryptionService
    {

        public EncryptedValue Encrypt(string value, IOutgoingLogicalMessageContext context)
        {
            throw new System.NotImplementedException();
        }

        public string Decrypt(EncryptedValue encryptedValue, IIncomingLogicalMessageContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}