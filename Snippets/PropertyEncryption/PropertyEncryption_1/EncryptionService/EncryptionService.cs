namespace Core6.Encryption.EncryptionService
{
    using System;

    #region EncryptionService

    using NServiceBus.Pipeline;
    using EncryptedValue = NServiceBus.Encryption.MessageProperty.EncryptedValue;
    using IEncryptionService = NServiceBus.Encryption.MessageProperty.IEncryptionService;

    public class EncryptionService :
        IEncryptionService
    {
        public EncryptedValue Encrypt(string value, IOutgoingLogicalMessageContext context)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(EncryptedValue encryptedValue, IIncomingLogicalMessageContext context)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}